# RBAC and audit layer

Apply `scripts/002_rbac_permissions.sql` to the `PressDMS` database after running the base schema (`~vs7E75.sql`). It adds the `Permissions` catalog and `RolePermissions` mapping, then idempotently seeds the existing Admin, Editor, and Accountant grants. Re-running this migration does not duplicate grants. Review grants against your organization's policy before deployment.

The application reads grants from these tables and fails closed if permission lookup fails. Administrators can assign a user's existing role and activate/deactivate accounts from the Users dashboard screen. The Audit Log screen is restricted to the `AuditRead` permission and shows the latest 500 records.

Document visibility in the desktop app is scoped as follows: Admin and Accountant users with `DocumentsRead` see all documents; Editors see only documents they uploaded. Search applies the same filter. An inactive account is rejected by the existing sign-in flow.

## Security boundary warning

This application currently connects directly to SQL Server using the connection string in `App.config`. Permission checks in a desktop client are useful application controls, but are not a trustworthy security boundary if users can modify the client or connect to SQL Server independently. For production, use a trusted backend/API or SQL Server identities with least-privilege grants and enforce authorization there. Do not rely on the client-supplied role value as the sole database authorization mechanism.

Run the project on Windows with .NET Framework 4.7.2 and verify the SQL migration and role behavior against a non-production database before rollout.
