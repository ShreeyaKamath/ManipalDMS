# ManipalDMS

ManipalDMS is a Windows desktop document management application built with C# Windows Forms and SQL Server. It includes a login screen and dashboard, document upload/download and search, and forms for managing advertisement and publishing records.

## Features

- Browse, upload, and download documents. Files are copied to a local storage folder; document metadata is stored in SQL Server.
- Search documents by title, category, uploader, and date range.
- View and add advertisement and publishing records.
- Record audit entries for document uploads, downloads, and searches.
- Display the signed-in user's role on the dashboard.

> **Current implementation note:** The role is displayed after login, but role-based restrictions on individual actions are not apparent in the current UI code. The document storage location and SQL Server connection are configured in source code. Review and update these settings before use. This project should be treated as a learning/demo application, not deployed with sensitive data as-is.

## Requirements

- Windows
- Visual Studio with .NET desktop development tools
- .NET Framework 4.7.2 Developer Pack (the project targets .NET Framework 4.7.2)
- SQL Server or SQL Server Express, with a database named `PressDMS`

## Setup

1. Clone the repository and open `ManipalDMS.sln` in Visual Studio.
2. Set up SQL Server and create the `PressDMS` database and required tables. The repository includes a sample schema and seed-data script in `~vs7E75.sql`.
   - **Caution:** That script drops and recreates the `PressDMS` database if it already exists. Back up any data you need and review the script before running it.
   - The script inserts sample users (`admin`, `editor`, and `accountant`) with demo passwords. These credentials are for local testing only; replace them and do not use them in a real deployment.
3. Configure the SQL Server connection in `DatabaseHelper.cs` to match your local server and database. Do not commit passwords or other secrets. Although `App.config` also contains a connection string, the current `DatabaseHelper` uses its own hard-coded connection string.
4. In `DocumentsForm.cs`, change `docsDirectory` to a folder that exists or that the application account can create and write to. The default is `C:\\ManipalDMSFiles\\`.
5. Build and run the solution in Visual Studio. The default database connection targets the local SQL Server Express instance (`.\\SQLEXPRESS`).

## Basic use

Sign in with a user present in the `Users` table. From the dashboard, open Documents to upload, download, refresh, or search for documents; use the other dashboard forms to work with advertisement and publishing records.

## Security considerations

This codebase is not production-ready. In particular, review the following before using real or sensitive data:

- Database credentials are embedded in source/configuration. Use protected configuration or a secrets manager and a least-privilege SQL account; never use a `sysadmin` account for the application.
- Login and data operations currently build SQL statements from user input. Replace them with parameterized commands and use secure password hashing rather than storing/comparing plaintext passwords.
- Configure access controls for the document storage folder and implement/verify authorization for each role and operation.
- Review file naming, overwrite behavior, validation, error handling, and audit-log coverage for your deployment requirements.

## Project structure

- `ManipalDMS.sln` / `ManipalDMS.csproj` — Visual Studio solution and project
- `Program.cs` — application entry point
- `LoginForm.cs`, `DashboardForm.cs` — login and navigation
- `DocumentsForm.cs` — document operations and search
- `AdsForm.cs`, `PublishingForm.cs` — advertisement and publishing records
- `DatabaseHelper.cs` — SQL Server access and audit logging
- `~vs7E75.sql` — sample database schema and seed data (review before running)

## License

See [LICENSE](LICENSE).
