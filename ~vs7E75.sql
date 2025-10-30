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
    Title NVARCHAR(100),
    Category NVARCHAR(50),
    FilePath NVARCHAR(255),
    UploadedBy NVARCHAR(50),
    UploadDate DATETIME DEFAULT GETDATE()
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

DELETE FROM Users;
GO
INSERT INTO Users (Username, Password, Role)
VALUES
('admin', 'admin123', 'Admin'),
('editor', 'editor123', 'Editor'),
('accountant', 'acct123', 'Accountant');


-- Sample documents
INSERT INTO Documents (Title, Category, FilePath, UploadedBy)
VALUES
('Marketing Strategy', 'Advertisement', 'C:\\Docs\\Marketing.pdf', 'editor'),
('Financial Report', 'Accounts', 'C:\\Docs\\Finance.xlsx', 'accountant');

-- Sample advertisements
INSERT INTO Advertisements (ClientName, AdType, Cost)
VALUES
('ABC Corp', 'Newspaper Ad', 2500.00),
('XYZ Pvt Ltd', 'Online Banner', 1800.00);

-- Sample publishing data
INSERT INTO Publishing (ArticleTitle, PublishDate, Cost, Revenue)
VALUES
('Annual Review 2025', GETDATE(), 5000.00, 8000.00),
('Tech Insights', GETDATE(), 3000.00, 5000.00);




SELECT * FROM Users;
SELECT * FROM Documents;
SELECT * FROM Advertisements;
SELECT * FROM Publishing;


ALTER TABLE Documents ADD StoredFileName NVARCHAR(255);
ALTER TABLE Documents ADD FileContent VARBINARY(MAX); -- Optional: store content in DB


CREATE TABLE AuditLog (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50),
    Action NVARCHAR(50),
    TableAffected NVARCHAR(50),
    Timestamp DATETIME DEFAULT GETDATE(),
    Details NVARCHAR(255));
