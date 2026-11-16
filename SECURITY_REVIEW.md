# Code review: remaining high-risk areas

Reviewed against branch `rbac-enhancement` at commit `f7038c560b1432b2abd43359e9cb075672d56ea0`.

## Findings

### Critical architecture limitation: desktop identity is not trusted
The WinForms client connects directly to SQL Server and sends application usernames/roles in parameters. A modified client can change those values or issue its own SQL. SQL procedures that accept `@ActorUsername` improve consistency but do not authenticate that actor. Production authorization needs a trusted API/service or a SQL identity per person with server-side identity-to-user mapping and EXECUTE-only permissions.

### High: several actions still perform direct table SQL
Document uploads/search, ads/publishing actions, user administration, login counters, and audit writes have historically used direct SQL from forms. New procedures in `scripts/004_authorized_data_procedures.sql` provide server-side checks for document, ads, publishing operations and last-active-admin protection, but those procedures must be wired into the application and granted to a trusted service identity before they provide an enforced boundary. Do not claim production DB enforcement until then.

### High: document-file access must be re-authorized
Filtering a grid is not sufficient: download must re-fetch a document by ID for the current authenticated principal and verify ownership/permission before reading its stored filename. The new `dbo.GetVisibleDocumentFile` procedure implements that check, but the client must call it and use its returned filename rather than trusting a selected grid row.

### High: admin actions need atomic server-side protection
The client-side user-management screen can be stale and role changes/deactivations may race. Use `dbo.AdminSetUserRole` and `dbo.AdminSetUserActive`; these reject unauthenticated-by-permissions actors and prevent removing/deactivating the last active Admin within a transaction. The actor identity parameter still requires a trusted caller.

### Medium: audit logging is not transactionally complete
Forms that write a record and then separately write an audit row can leave one without the other. The new data procedures include audit rows only for admin role/status changes. For all business mutations, write the audit event in the same SQL transaction/procedure. Avoid logging secrets or file contents.

### Medium: login lockout is client-managed
The login flow checks active/lockout fields and uses PBKDF2, but direct client lockout updates are vulnerable to bypass and race conditions. Use a trusted auth service or database-authenticated identity, rate limiting, monitored lockouts, and account recovery policy.

### Medium: project build/test evidence
Repository write tools cannot run Visual Studio/MSBuild, SQL Server migrations, or Windows ACL tests. Build in Release on Windows and test scripts against a disposable SQL Server before deployment. Add automated tests for permission grants, document ownership, inactive/locked users, admin-last-account protection, and stored-procedure failure cases.

## Recommended order

1. Decide trusted principal model (API/service or per-user SQL identity). Do not use shared SQL identity plus a caller-supplied username as a security boundary.
2. Apply scripts 002, 003 and 004 to a restored test database; verify all role grants and transactional behaviors.
3. Refactor each form to call authorized stored procedures only; remove broad direct table permissions.
4. Add integration tests for positive and negative authorization cases, then run a Windows Release build and deployment checklist.
5. Review storage ACLs, secrets, backups, audit retention, SQL network exposure, and incident response.
