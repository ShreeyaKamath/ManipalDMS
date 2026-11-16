USE master;
GO

IF DB_ID(N'PressDMS') IS NULL
BEGIN
    CREATE DATABASE PressDMS;
END
GO

USE PressDMS;
GO

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash VARBINARY(64) NOT NULL,
    PasswordSalt VARBINARY(32) NOT NULL,
    PasswordAlgorithm NVARCHAR(20) NOT NULL DEFAULT 'PBKDF2',
    Role NVARCHAR(20) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    FailedLoginAttempts INT NOT NULL DEFAULT 0,
    LockoutUntilUtc DATETIME2 NULL,
    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE Documents (
    DocumentID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    FilePath NVARCHAR(255) NOT NULL,
    StoredFileName NVARCHAR(255) NOT NULL,
    UploadedBy NVARCHAR(50) NOT NULL,
    UploadDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE Advertisements (
    AdID INT IDENTITY(1,1) PRIMARY KEY,
    ClientName NVARCHAR(100) NOT NULL,
    AdType NVARCHAR(50) NOT NULL,
    Cost DECIMAL(10,2) NOT NULL,
    DatePosted DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE Publishing (
    PubID INT IDENTITY(1,1) PRIMARY KEY,
    ArticleTitle NVARCHAR(100) NOT NULL,
    PublishDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Cost DECIMAL(10,2) NOT NULL,
    Revenue DECIMAL(10,2) NOT NULL
);

CREATE TABLE AuditLog (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Action NVARCHAR(50) NOT NULL,
    TableAffected NVARCHAR(50) NOT NULL,
    Timestamp DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Details NVARCHAR(255) NOT NULL
);

-- The application should provision users through a secure admin workflow.
-- Production authentication must use strong password hashing and environment-managed secrets.
-- Do not seed demo credentials or sample password values into production code repositories.
GO
