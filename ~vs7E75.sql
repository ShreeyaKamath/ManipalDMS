USE master;
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'PressDMS')
BEGIN
    ALTER DATABASE PressDMS SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PressDMS;
END
GO
CREATE DATABASE PressDMS;
GO
USE PressDMS;
GO

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    Role NVARCHAR(20) NOT NULL
);

CREATE TABLE Documents (
    DocumentID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    FilePath NVARCHAR(255) NOT NULL,
    StoredFileName NVARCHAR(255) NOT NULL,
    UploadedBy NVARCHAR(50) NOT NULL,
    UploadDate DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Advertisements (
    AdID INT IDENTITY(1,1) PRIMARY KEY,
    ClientName NVARCHAR(100),
    AdType NVARCHAR(50),
    Cost DECIMAL(10,2),
    DatePosted DATETIME DEFAULT GETDATE()
);

CREATE TABLE Publishing (
    PubID INT IDENTITY(1,1) PRIMARY KEY,
    ArticleTitle NVARCHAR(100),
    PublishDate DATETIME DEFAULT GETDATE(),
    Cost DECIMAL(10,2),
    Revenue DECIMAL(10,2)
);

CREATE TABLE AuditLog (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50),
    Action NVARCHAR(50),
    TableAffected NVARCHAR(50),
    Timestamp DATETIME DEFAULT GETDATE(),
    Details NVARCHAR(255)
);

-- Local demo accounts only. Replace with securely managed, hashed credentials before deployment.
INSERT INTO Users (Username, Password, Role)
VALUES ('admin', 'admin123', 'Admin'),
       ('editor', 'editor123', 'Editor'),
       ('accountant', 'acct123', 'Accountant');

-- Sample metadata. These example files are not included in the repository.
INSERT INTO Documents (Title, Category, FilePath, StoredFileName, UploadedBy)
VALUES ('Marketing Strategy', 'Advertisement', 'C:\\Docs\\Marketing.pdf', 'Marketing.pdf', 'editor'),
       ('Financial Report', 'Accounts', 'C:\\Docs\\Finance.xlsx', 'Finance.xlsx', 'accountant');

INSERT INTO Advertisements (ClientName, AdType, Cost)
VALUES ('ABC Corp', 'Newspaper Ad', 2500.00),
       ('XYZ Pvt Ltd', 'Online Banner', 1800.00);

INSERT INTO Publishing (ArticleTitle, PublishDate, Cost, Revenue)
VALUES ('Annual Review 2025', GETDATE(), 5000.00, 8000.00),
       ('Tech Insights', GETDATE(), 3000.00, 5000.00);
GO
