# ManipalDMS RBAC operations

## Apply database scripts

For a clean, disposable database, run the base schema `~vs7E75.sql`, then `scripts/002_rbac_permissions.sql`, then `scripts/003_user_provisioning.sql`. For an existing database, back it up and test reviewed migrations on a restored copy first; the base script is not a data-conversion migration.

The permission catalog and role grants are stored in `dbo.Permissions` and `dbo.RolePermissions`. Default grants are Admin (all seeded permissions), Editor (document read/write), and Accountant (document read/read-all/write, ads read/write, publishing read/write). The RBAC seed script is idempotent and does not remove additional grants.

The user provisioning procedure accepts a PBKDF2 hash/salt, not a plaintext password. In the client, the password is hashed before the procedure call. No sample account credentials are seeded.

## Access behavior

- Module navigation is based on Read permission; write buttons/actions require the separate Write grant.
- `DocumentsReadAll` grants cross-uploader document visibility. Editors do not receive it and are restricted to their own uploads in list/search/count queries.
- User administration requires both UsersRead and UsersWrite. Audit browsing requires AuditRead.
- Login rejects inactive users, unsupported password algorithms, locked accounts, and roles with no configured permission grants.
- Permission lookup errors deny access.

## Security limitation

This WinForms program currently connects directly to SQL Server, and its forms execute table SQL. Client-side checks and a role string are not a trustworthy database boundary. A modified client can bypass those checks. For production, enforce authorization and document ownership in a trusted API/service or in SQL procedures under an identity model that can securely identify the individual user. Do not grant end users broad table permissions or rely on a client-supplied role.
