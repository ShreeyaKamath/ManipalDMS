# ManipalDMS

ManipalDMS is a Windows desktop document management application built with C# Windows Forms and SQL Server. It provides sign-in, document management, advertisement tracking, and publishing workflow.

## Security and RBAC

The application uses PBKDF2-SHA256 password hashes, failed-login lockouts, database-backed role permission grants, role-aware module access, document uploader filtering for Editors, admin user management, and an audit log viewer with username/action/date filters. These controls are defense in depth for the desktop UI, not a substitute for trusted backend/database authorization.

## Setup

1. Open `ManipalDMS.sln` in Visual Studio with the .NET Framework 4.7.2 Developer Pack.
2. Run `~vs7E75.sql` in a non-production SQL Server database.
3. Run `scripts/002_rbac_permissions.sql` and `scripts/003_user_provisioning.sql` in that order.
4. Configure the `PressDMS` connection string in `App.config` for your deployment. Do not commit credentials.
5. Build and run the solution. Create the initial administrator through a separately controlled provisioning workflow; scripts do not create an account or password.
6. Verify user roles, disabled account rejection, document filtering, audit filtering, and module read/write grants before deployment.

For details and security limitations, see [RBAC.md](RBAC.md) and [DEPLOYMENT.md](DEPLOYMENT.md).
