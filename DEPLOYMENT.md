# Secure deployment checklist

This application is a Windows Forms desktop client connecting directly to SQL Server. UI-side authorization is not a trusted security boundary. Do not deploy it for sensitive multi-user data until access is enforced by a trusted API/service or a SQL execution model that can reliably identify each user and restrict operations. The deployment steps below reduce risk but do not change that architectural limitation.

## Before deployment

- [ ] Work from a reviewed commit/branch; build the Release configuration on a Windows machine with the .NET Framework 4.7.2 Developer Pack.
- [ ] Review the complete SQL scripts and source; no database script in this project should be run against production without an approved backup, change window, and rollback plan.
- [ ] Use a disposable SQL Server database first. For a clean install run `~vs7E75.sql`, then `scripts/002_rbac_permissions.sql`, then `scripts/003_user_provisioning.sql`.
- [ ] If an existing database is involved, treat these as migrations, back it up, compare the live schema/data, and test the migration on a restored copy. Do not run the clean-install base script as an automatic data conversion.
- [ ] Review the seeded role grants in `scripts/002_rbac_permissions.sql`. Expected defaults: Admin all grants; Editor document read/write only; Accountant documents (including all-document read), ads, and publishing.
- [ ] Create no demo accounts or shared passwords. Provision an initial admin using a separately controlled process; require a unique account and a strong, non-reused password.
- [ ] Ensure SQL Server is patched, uses TLS with certificate validation, and is not exposed to untrusted networks.
- [ ] Do not use `sa`, `sysadmin`, `db_owner`, or a shared privileged SQL identity for the client. Restrict SQL firewall/network access.
- [ ] Keep real connection strings and secrets out of Git. Use Windows/enterprise secret management; use `Encrypt=True` and certificate validation, never production `TrustServerCertificate=True`.
- [ ] Give the document folder least-privilege NTFS ACLs with `scripts/Set-DocumentStorageAcl.ps1`. Confirm all client users that need file access have only the required access.
- [ ] Use malware scanning, encrypted storage/backups, retention controls, and tested restore procedures for uploaded documents and the database.

## Verify in a test environment

- [ ] Apply migrations and rerun them to confirm idempotence; inspect grants and foreign keys.
- [ ] Test Admin, Editor, and Accountant accounts separately, including Read without Write grants.
- [ ] Confirm an inactive account and a user with an unsupported password algorithm are denied at login.
- [ ] Confirm a role without configured permissions is denied at login and permission lookup errors fail closed.
- [ ] Confirm Editors only see their own documents in lists, searches, downloads, and dashboard counts; Admin/Accountant all-document access comes from the `DocumentsReadAll` grant.
- [ ] Confirm ads and publishing read/write permissions are independently enforced, and user/audit functions remain restricted.
- [ ] Test invalid inputs, oversized/disallowed files, storage permission failures, database outages, and audit failures.
- [ ] Review logs for sensitive data: do not log plaintext passwords, hashes, salts, or connection strings.
- [ ] Build and run all workflows on the target Windows/.NET Framework environment. GitHub file-write operations do not run MSBuild or SQL integration tests.

## Operational controls

- [ ] Assign a named owner for role grants, account provisioning/deactivation, audit review, and incident response.
- [ ] Review access periodically; promptly disable departing users and revoke credentials.
- [ ] Monitor failed logins, role changes, user status changes, document transfers, and privileged actions.
- [ ] Back up before schema/grant changes and document rollback steps.
- [ ] Code-sign release binaries, distribute through a controlled channel, and keep OS/.NET/SQL patches current.
- [ ] Rotate any credentials that were ever committed; deleting files does not remove secrets from Git history.

## Architectural follow-up required for strong security

Replace direct client-to-database access with a trusted service/API or carefully designed stored procedures and per-user database identity. Enforce role checks, ownership rules, and audit writes at that boundary. The current client passes role strings and executes table SQL; a modified client can bypass those checks, so these changes are defense-in-depth/UI behavior, not a claim of server-enforced RBAC.
