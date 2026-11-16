USE master;
GO
IF DB_ID(N'PressDMS') IS NULL CREATE DATABASE PressDMS;
GO
USE PressDMS;
GO

IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles (
        RoleID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Roles PRIMARY KEY,
        RoleName NVARCHAR(30) NOT NULL CONSTRAINT UQ_Roles_RoleName UNIQUE,
        Description NVARCHAR(200) NULL
    );
END;
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = N'Admin') INSERT dbo.Roles(RoleName, Description) VALUES (N'Admin', N'Administrative access');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = N'Editor') INSERT dbo.Roles(RoleName, Description) VALUES (N'Editor', N'Document access');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = N'Accountant') INSERT dbo.Roles(RoleName, Description) VALUES (N'Accountant', N'Financial record access');
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
CREATE TABLE dbo.Users (
    UserID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL CONSTRAINT UQ_Users_Username UNIQUE,
    PasswordHash VARBINARY(32) NOT NULL,
    PasswordSalt VARBINARY(32) NOT NULL,
    PasswordAlgorithm NVARCHAR(30) NOT NULL CONSTRAINT DF_Users_Algorithm DEFAULT N'PBKDF2-SHA256-310000',
    Role NVARCHAR(30) NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT 1,
    FailedLoginAttempts INT NOT NULL CONSTRAINT DF_Users_Failed DEFAULT 0,
    LockoutUntilUtc DATETIME2 NULL,
    CreatedUtc DATETIME2 NOT NULL CONSTRAINT DF_Users_Created DEFAULT SYSUTCDATETIME(),
    UpdatedUtc DATETIME2 NOT NULL CONSTRAINT DF_Users_Updated DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Users_Roles FOREIGN KEY (Role) REFERENCES dbo.Roles(RoleName)
);
GO

IF OBJECT_ID(N'dbo.Documents', N'U') IS NULL
CREATE TABLE dbo.Documents (
    DocumentID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Documents PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL, Category NVARCHAR(50) NOT NULL,
    FilePath NVARCHAR(255) NOT NULL, StoredFileName NVARCHAR(255) NOT NULL,
    UploadedBy NVARCHAR(50) NOT NULL, UploadDate DATETIME2 NOT NULL CONSTRAINT DF_Documents_UploadDate DEFAULT SYSUTCDATETIME()
);
IF OBJECT_ID(N'dbo.Advertisements', N'U') IS NULL
CREATE TABLE dbo.Advertisements (
    AdID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Advertisements PRIMARY KEY,
    ClientName NVARCHAR(100) NOT NULL, AdType NVARCHAR(50) NOT NULL,
    Cost DECIMAL(10,2) NOT NULL, DatePosted DATETIME2 NOT NULL CONSTRAINT DF_Advertisements_Date DEFAULT SYSUTCDATETIME()
);
IF OBJECT_ID(N'dbo.Publishing', N'U') IS NULL
CREATE TABLE dbo.Publishing (
    PubID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Publishing PRIMARY KEY,
    ArticleTitle NVARCHAR(100) NOT NULL,
    PublishDate DATETIME2 NOT NULL CONSTRAINT DF_Publishing_Date DEFAULT SYSUTCDATETIME(),
    Cost DECIMAL(10,2) NOT NULL, Revenue DECIMAL(10,2) NOT NULL
);
IF OBJECT_ID(N'dbo.AuditLog', N'U') IS NULL
CREATE TABLE dbo.AuditLog (
    LogID BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLog PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL, Action NVARCHAR(50) NOT NULL,
    TableAffected NVARCHAR(50) NOT NULL, [Timestamp] DATETIME2 NOT NULL CONSTRAINT DF_AuditLog_Timestamp DEFAULT SYSUTCDATETIME(),
    Details NVARCHAR(255) NOT NULL
);
GO

-- No demo accounts, passwords, SQL logins, database users, or permissions are created here.
-- Existing demo databases require a separately reviewed migration and password reset; do not run this as a data conversion script.
