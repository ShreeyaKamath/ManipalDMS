# ManipalDMS

ManipalDMS is a Windows desktop document management application built with C# Windows Forms and SQL Server. The application provides sign-in, document management, advertisement tracking, and publishing records.

## Interactive features

- Sign in against the `Users` table; the signed-in username is used for document ownership and audit entries.
- Upload documents, record their metadata, search by title/category/uploader/date range, refresh results, and download the selected file (or double-click a row).
- Add and review advertisement and publishing records with input validation and automatic list refresh.
- Record audit entries for uploads, downloads, searches, and newly created business records.
- SQL operations use parameters, and uploaded files are stored under the current Windows user's Local AppData folder in `ManipalDMS/Documents`.

## Requirements

- Windows
- Visual Studio with .NET desktop development tools
- .NET Framework 4.7.2 Developer Pack
- SQL Server or SQL Server Express

## Setup

1. Open `ManipalDMS.sln` in Visual Studio.
2. Review `~vs7E75.sql` before running it in SQL Server Management Studio. **It drops and recreates the `PressDMS` database.** Back up any existing data first. The script creates the tables and local demo accounts.
3. In `App.config`, update the `PressDMS` connection string's server name if needed. It uses Windows Integrated Security; grant the Windows account running the app appropriate database permissions.
4. Build and run the solution. Demo credentials from the seed script: `admin` / `admin123`, `editor` / `editor123`, or `accountant` / `acct123`.

## Important security notes

This is a local/demo application, not production-ready. The sample schema stores demo passwords as plaintext; replace this with a vetted password-hashing and account-management design before using real accounts. Apply least-privilege SQL permissions, validate deployment-specific role authorization, and secure the local document storage folder. Do not use the sample credentials or destructive seed script with sensitive data.

## Project structure

- `ManipalDMS.sln` / `ManipalDMS.csproj` — Visual Studio solution and project
- `Program.cs` — application entry point
- `LoginForm.cs`, `DashboardForm.cs` — sign-in and navigation
- `DocumentsForm.cs` — upload, download, and search
- `AdsForm.cs`, `PublishingForm.cs` — business record entry and display
- `DatabaseHelper.cs` — parameterized SQL Server access and audit logging
- `~vs7E75.sql` — sample database schema and seed data (review before running)

## License

See [LICENSE](LICENSE).
