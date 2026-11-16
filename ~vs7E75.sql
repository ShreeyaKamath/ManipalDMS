USE master;
GO

IF DB_ID(N'PressDMS') IS NULL
BEGIN
    CREATE DATABASE PressDMS;
END
GO

USE PressDMS;
GO

CREATE TABLE dbo.Roles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(30) NOT NULL UNIQUE,
    Description NVARCHAR(200) NULL
);
GO

INSERT INTO dbo.Roles (RoleName, Description)
VALUES
    (N'Admin', N'Full administrative access'),
    (N'Editor', N'Can manage documents and metadata'),
    (N'Accountant', N'Can manage financial records');
GO

CREATE TABLE dbo.Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARBINARY(64) NOT NULL,
    PasswordSalt VARBINARY(32) NOT NULL,
    PasswordAlgorithm NVARCHAR(20) NOT NULL DEFAULT N'PBKDF2',
    Role NVARCHAR(30) NOT NULL DEFAULT N'Editor',
    IsActive BIT NOT NULL DEFAULT 1,
    FailedLoginAttempts INT NOT NULL DEFAULT 0,
    LockoutUntilUtc DATETIME2 NULL,
    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Users_Roles FOREIGN KEY (Role) REFERENCES dbo.Roles(RoleName)
);
GO

CREATE TABLE dbo.Documents (
    DocumentID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(150) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    StoredFileName NVARCHAR(255) NOT NULL,
    UploadedBy NVARCHAR(50) NOT NULL,
    UploadDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE dbo.Advertisements (
    AdID INT IDENTITY(1,1) PRIMARY KEY,
    ClientName NVARCHAR(150) NOT NULL,
    AdType NVARCHAR(80) NOT NULL,
    Cost DECIMAL(12,2) NOT NULL,
    DatePosted DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE dbo.Publishing (
    PubID INT IDENTITY(1,1) PRIMARY KEY,
    ArticleTitle NVARCHAR(150) NOT NULL,
    PublishDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Cost DECIMAL(12,2) NOT NULL,
    Revenue DECIMAL(12,2) NOT NULL
);
GO

CREATE TABLE dbo.AuditLog (
    LogID BIGINT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Action NVARCHAR(50) NOT NULL,
    TableAffected NVARCHAR(50) NOT NULL,
    Timestamp DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Details NVARCHAR(500) NOT NULL
);
GO

-- IMPORTANT: Create a dedicated application login via your enterprise provisioning or secret-management workflow.
-- Do not assign sysadmin or SA privileges to the application.
CREATE USER [ManipalDMS_AppUser] FOR LOGIN [ManipalDMS_AppUser];
GO

GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Users TO [ManipalDMS_AppUser];
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Documents TO [ManipalDMS_AppUser];
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Advertisements TO [ManipalDMS_AppUser];
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Publishing TO [ManipalDMS_AppUser];
GRANT SELECT, INSERT ON dbo.AuditLog TO [ManipalDMS_AppUser];
GO
