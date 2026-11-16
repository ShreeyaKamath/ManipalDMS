# ManipalDMS

ManipalDMS is a Windows desktop document management application built with C# Windows Forms and SQL Server. It provides sign-in, document management, advertisement tracking, and publishing workflows with audit logging and role-based access control.

## Production readiness status

This repository has been hardened for production-oriented deployment by removing destructive SQL setup steps, enforcing hashed password verification, narrowing file upload risk, and standardizing secure configuration.

## Key security changes

- Password verification uses PBKDF2 hashing with a per-user salt instead of plaintext database comparison.
- The sample SQL scripts no longer drop the database or create privileged SA accounts with hardcoded credentials.
- The document storage directory uses the shared application data location instead of the current user profile folder.
- File upload validation restricts file type and size.
- Application configuration is environment-specific and avoids insecure local defaults.
- Role-based authorization is enforced in the app before exposing management screens.

## Interactive features

- Sign in against the `Users` table using a hashed password and role lookup.
- Upload documents, record metadata, search by title/category/uploader/date range, refresh results, and download the selected file.
- Add and review advertisement and publishing records with input validation and automatic list refresh.
- Record audit entries for uploads, downloads, searches, and newly created business records.
- Uploaded files are stored under the secure shared application folder in `ProgramData\ManipalDMS\Documents`.

## Requirements

- Windows
- Visual Studio with .NET desktop development tools
- .NET Framework 4.7.2 Developer Pack
- SQL Server or SQL Server Express

## Setup

1. Open `ManipalDMS.sln` in Visual Studio.
2. Review and run `~vs7E75.sql` in SQL Server Management Studio. The script creates the production-safe schema without dropping the database. Back up any existing data before applying a migration.
3. Update `App.config` with the deployment-specific SQL Server name and authentication settings.
4. Build and run the solution.
5. Create application users through a secure provisioning workflow and assign roles through the database or an admin process.

## Important security notes

- Never store plaintext passwords in the application or database.
- Do not commit real credentials, connection strings, or secret values to source control.
- Provision SQL logins and permissions using a least-privilege process managed by your environment.
- Treat the sample schema as a starting template only; it must be replaced with your secure identity and access-control design before production use.
- Review file storage permissions on the `ProgramData\ManipalDMS\Documents` directory before deployment.

## Project structure

- `ManipalDMS.sln` / `ManipalDMS.csproj` — Visual Studio solution and project
- `Program.cs` — application entry point
- `LoginForm.cs`, `DashboardForm.cs` — sign-in and navigation
- `DocumentsForm.cs` — upload, download, and search
- `AdsForm.cs`, `PublishingForm.cs` — business record entry and display
- `DatabaseHelper.cs` — parameterized SQL Server access and audit logging
- `PasswordHashing.cs` — PBKDF2 hashing and verification
- `SecurityPolicy.cs` — centralized role authorization rules
- `~vs7E75.sql` — secure schema creation script
- `~vs2C9E.sql` — intentionally empty of credentials; use your enterprise provisioning workflow instead

## License

See [LICENSE](LICENSE).
