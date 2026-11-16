# ManipalDMS RBAC operations

## Apply database scripts

1. Run the base schema `~vs7E75.sql` on a non-production database.
2. Run `scripts/002_rbac_permissions.sql` to create the permission catalog and seed default role grants.
3. Run `scripts/003_user_provisioning.sql` to add the secure account-provisioning stored procedure.
4. Review the grants and deployment identity before connecting the application.

The provisioning procedure accepts a PBKDF2 hash and salt, not a plaintext password. Admins can create accounts in the Users screen; the app hashes the password using PBKDF2-SHA256 (310,000 iterations) before calling the procedure. No real accounts or credentials are seeded by these scripts.

## Access behavior

- Admin has all seeded permissions, including user administration and audit access.
- Editor can read/write documents; document rows are scoped to the current uploader in the application.
- Accountant can read/write documents, advertisements, and publishing records; the current document policy allows accountants to view all documents.
- Ads and publishing screens check their respective database-backed Read and Write grants. A role without Read cannot open the module; a role with Read but without Write sees records but cannot add them.
- Audit Log is Admin-only by default and supports username, action, and date-range filtering, with a 500-row result cap.
- Disabled users are rejected at sign-in by the existing login flow.

## Security limitation

This WinForms program currently connects directly to SQL Server. UI and client-side permission checks can be bypassed by a modified client or direct database access. For production, put authorization in a trusted API/service or enforce it with carefully designed SQL identities, stored procedures, and least-privilege database grants. Do not give end-user accounts broad table access or treat a client-supplied role as a trusted database identity.

Test migrations and user provisioning in a disposable database before deployment. Ensure your SQL provider supports `CREATE OR ALTER PROCEDURE` (SQL Server 2016 SP1 or newer); on older versions deploy an equivalent reviewed procedure migration.
