USE PressDMS;
GO

-- Admin-only account provisioning procedure. Supply PBKDF2 hash/salt computed by the application;
-- this procedure intentionally accepts no plaintext password and seeds no example accounts.
CREATE OR ALTER PROCEDURE dbo.CreateApplicationUser
    @Username NVARCHAR(50),
    @PasswordHash VARBINARY(32),
    @PasswordSalt VARBINARY(32),
    @Role NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NULLIF(LTRIM(RTRIM(@Username)), N'') IS NULL
        THROW 50001, 'Username is required.', 1;
    IF DATALENGTH(@PasswordHash) <> 32 OR DATALENGTH(@PasswordSalt) <> 32
        THROW 50002, 'Password hash and salt must each be 32 bytes.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = @Role)
        THROW 50003, 'The requested role does not exist.', 1;
    IF EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        THROW 50004, 'The username is already in use.', 1;

    INSERT dbo.Users (Username, PasswordHash, PasswordSalt, PasswordAlgorithm, Role, IsActive)
    VALUES (LTRIM(RTRIM(@Username)), @PasswordHash, @PasswordSalt, N'PBKDF2-SHA256-310000', @Role, 1);
END;
GO

-- Provision database permissions for deployment accounts separately. Do not grant this procedure
-- to general users; grant EXECUTE only to the trusted application/service identity after reviewing
-- the deployment's identity and authorization design.
