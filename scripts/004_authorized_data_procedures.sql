USE PressDMS;
GO

-- SQL-side authorization procedures for defense in depth. They resolve role grants from dbo.Users,
-- not from a role/permission flag supplied by the UI. @ActorUsername must come from a trusted
-- authenticated service; a desktop client that can choose this value is NOT a secure identity boundary.

CREATE OR ALTER PROCEDURE dbo.GetVisibleDocuments
    @ActorUsername NVARCHAR(50),
    @FromUtc DATETIME2 = NULL,
    @ToUtc DATETIME2 = NULL,
    @Title NVARCHAR(100) = NULL,
    @Category NVARCHAR(50) = NULL,
    @Uploader NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CanRead BIT = 0, @CanReadAll BIT = 0;
    SELECT
        @CanRead = MAX(CASE WHEN p.PermissionKey = N'DocumentsRead' THEN 1 ELSE 0 END),
        @CanReadAll = MAX(CASE WHEN p.PermissionKey = N'DocumentsReadAll' THEN 1 ELSE 0 END)
    FROM dbo.Users u
    INNER JOIN dbo.Roles r ON r.RoleName = u.Role
    INNER JOIN dbo.RolePermissions rp ON rp.RoleID = r.RoleID
    INNER JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
    WHERE u.Username = @ActorUsername AND u.IsActive = 1;

    IF ISNULL(@CanRead, 0) = 0
        THROW 50101, 'Document read permission is required.', 1;

    SELECT d.DocumentID, d.Title, d.Category, d.UploadedBy, d.UploadDate, d.StoredFileName
    FROM dbo.Documents d
    WHERE (@CanReadAll = 1 OR d.UploadedBy = @ActorUsername)
      AND (@FromUtc IS NULL OR d.UploadDate >= @FromUtc)
      AND (@ToUtc IS NULL OR d.UploadDate < @ToUtc)
      AND (@Title IS NULL OR d.Title LIKE N'%' + @Title + N'%')
      AND (@Category IS NULL OR d.Category LIKE N'%' + @Category + N'%')
      AND (@Uploader IS NULL OR (@CanReadAll = 1 AND d.UploadedBy LIKE N'%' + @Uploader + N'%'))
    ORDER BY d.UploadDate DESC, d.DocumentID DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.GetVisibleDocumentFile
    @ActorUsername NVARCHAR(50),
    @DocumentID INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CanRead BIT = 0, @CanReadAll BIT = 0;
    SELECT
        @CanRead = MAX(CASE WHEN p.PermissionKey = N'DocumentsRead' THEN 1 ELSE 0 END),
        @CanReadAll = MAX(CASE WHEN p.PermissionKey = N'DocumentsReadAll' THEN 1 ELSE 0 END)
    FROM dbo.Users u
    INNER JOIN dbo.Roles r ON r.RoleName = u.Role
    INNER JOIN dbo.RolePermissions rp ON rp.RoleID = r.RoleID
    INNER JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
    WHERE u.Username = @ActorUsername AND u.IsActive = 1;

    IF ISNULL(@CanRead, 0) = 0
        THROW 50102, 'Document read permission is required.', 1;

    SELECT d.DocumentID, d.StoredFileName
    FROM dbo.Documents d
    WHERE d.DocumentID = @DocumentID
      AND (@CanReadAll = 1 OR d.UploadedBy = @ActorUsername);
END;
GO

CREATE OR ALTER PROCEDURE dbo.AddDocumentMetadata
    @ActorUsername NVARCHAR(50),
    @Title NVARCHAR(100),
    @Category NVARCHAR(50),
    @FilePath NVARCHAR(255),
    @StoredFileName NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CanWrite BIT = 0;
    SELECT @CanWrite = CASE WHEN EXISTS
    (
        SELECT 1 FROM dbo.Users u
        INNER JOIN dbo.Roles r ON r.RoleName = u.Role
        INNER JOIN dbo.RolePermissions rp ON rp.RoleID = r.RoleID
        INNER JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
        WHERE u.Username = @ActorUsername AND u.IsActive = 1 AND p.PermissionKey = N'DocumentsWrite'
    ) THEN 1 ELSE 0 END;
    IF @CanWrite = 0 THROW 50103, 'Document write permission is required.', 1;

    INSERT dbo.Documents (Title, Category, FilePath, StoredFileName, UploadedBy)
    VALUES (@Title, @Category, @FilePath, @StoredFileName, @ActorUsername);
    SELECT CONVERT(INT, SCOPE_IDENTITY()) AS DocumentID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.GetVisibleAdvertisements
    @ActorUsername NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.Users u
        INNER JOIN dbo.Roles r ON r.RoleName = u.Role
        INNER JOIN dbo.RolePermissions rp ON rp.RoleID = r.RoleID
        INNER JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
        WHERE u.Username = @ActorUsername AND u.IsActive = 1 AND p.PermissionKey = N'AdsRead'
    ) THROW 50104, 'Advertisement read permission is required.', 1;

    SELECT AdID, ClientName, AdType, Cost, DatePosted
    FROM dbo.Advertisements ORDER BY DatePosted DESC, AdID DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.AddAdvertisement
    @ActorUsername NVARCHAR(50),
    @ClientName NVARCHAR(100),
    @AdType NVARCHAR(50),
    @Cost DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.Users u
        INNER JOIN dbo.Roles r ON r.RoleName = u.Role
        INNER JOIN dbo.RolePermissions rp ON rp.RoleID = r.RoleID
        INNER JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
        WHERE u.Username = @ActorUsername AND u.IsActive = 1 AND p.PermissionKey = N'AdsWrite'
    ) THROW 50105, 'Advertisement write permission is required.', 1;

    INSERT dbo.Advertisements (ClientName, AdType, Cost) VALUES (@ClientName, @AdType, @Cost);
END;
GO

CREATE OR ALTER PROCEDURE dbo.GetVisiblePublishing
    @ActorUsername NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.Users u
        INNER JOIN dbo.Roles r ON r.RoleName = u.Role
        INNER JOIN dbo.RolePermissions rp ON rp.RoleID = r.RoleID
        INNER JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
        WHERE u.Username = @ActorUsername AND u.IsActive = 1 AND p.PermissionKey = N'PublishingRead'
    ) THROW 50106, 'Publishing read permission is required.', 1;

    SELECT PubID, ArticleTitle, PublishDate, Cost, Revenue
    FROM dbo.Publishing ORDER BY PublishDate DESC, PubID DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.AddPublishingRecord
    @ActorUsername NVARCHAR(50),
    @ArticleTitle NVARCHAR(100),
    @Cost DECIMAL(10,2),
    @Revenue DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.Users u
        INNER JOIN dbo.Roles r ON r.RoleName = u.Role
        INNER JOIN dbo.RolePermissions rp ON rp.RoleID = r.RoleID
        INNER JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
        WHERE u.Username = @ActorUsername AND u.IsActive = 1 AND p.PermissionKey = N'PublishingWrite'
    ) THROW 50107, 'Publishing write permission is required.', 1;

    INSERT dbo.Publishing (ArticleTitle, Cost, Revenue) VALUES (@ArticleTitle, @Cost, @Revenue);
END;
GO

CREATE OR ALTER PROCEDURE dbo.AdminSetUserRole
    @ActorUsername NVARCHAR(50),
    @UserID INT,
    @NewRole NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;

    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.Users u
        INNER JOIN dbo.Roles r ON r.RoleName = u.Role
        INNER JOIN dbo.RolePermissions rp ON rp.RoleID = r.RoleID
        INNER JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
        WHERE u.Username = @ActorUsername AND u.IsActive = 1 AND p.PermissionKey = N'UsersWrite'
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50108, 'User administration permission is required.', 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = @NewRole)
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50109, 'Requested role does not exist.', 1;
    END;

    DECLARE @OldRole NVARCHAR(30);
    SELECT @OldRole = Role FROM dbo.Users WITH (UPDLOCK, HOLDLOCK) WHERE UserID = @UserID;
    IF @OldRole IS NULL
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50110, 'User account does not exist.', 1;
    END;

    IF @OldRole = N'Admin' AND @NewRole <> N'Admin'
       AND (SELECT COUNT(*) FROM dbo.Users WITH (UPDLOCK, HOLDLOCK) WHERE Role = N'Admin' AND IsActive = 1) <= 1
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50111, 'Cannot remove the last active administrator.', 1;
    END;

    UPDATE dbo.Users SET Role = @NewRole, UpdatedUtc = SYSUTCDATETIME() WHERE UserID = @UserID;
    INSERT dbo.AuditLog (Username, Action, TableAffected, Details)
    VALUES (@ActorUsername, N'RoleUpdated', N'Users', CONCAT(N'UserID=', @UserID, N'; NewRole=', @NewRole));
    COMMIT TRANSACTION;
END;
GO

CREATE OR ALTER PROCEDURE dbo.AdminSetUserActive
    @ActorUsername NVARCHAR(50),
    @UserID INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;

    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.Users u
        INNER JOIN dbo.Roles r ON r.RoleName = u.Role
        INNER JOIN dbo.RolePermissions rp ON rp.RoleID = r.RoleID
        INNER JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
        WHERE u.Username = @ActorUsername AND u.IsActive = 1 AND p.PermissionKey = N'UsersWrite'
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50112, 'User administration permission is required.', 1;
    END;

    DECLARE @TargetRole NVARCHAR(30), @TargetActive BIT;
    SELECT @TargetRole = Role, @TargetActive = IsActive
    FROM dbo.Users WITH (UPDLOCK, HOLDLOCK) WHERE UserID = @UserID;
    IF @TargetRole IS NULL
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50113, 'User account does not exist.', 1;
    END;

    IF @TargetRole = N'Admin' AND @TargetActive = 1 AND @IsActive = 0
       AND (SELECT COUNT(*) FROM dbo.Users WITH (UPDLOCK, HOLDLOCK) WHERE Role = N'Admin' AND IsActive = 1) <= 1
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50114, 'Cannot deactivate the last active administrator.', 1;
    END;

    UPDATE dbo.Users SET IsActive = @IsActive, UpdatedUtc = SYSUTCDATETIME() WHERE UserID = @UserID;
    INSERT dbo.AuditLog (Username, Action, TableAffected, Details)
    VALUES (@ActorUsername, N'UserStatusChanged', N'Users', CONCAT(N'UserID=', @UserID, N'; IsActive=', @IsActive));
    COMMIT TRANSACTION;
END;
GO

-- Review and apply EXECUTE-only grants for a trusted service identity in deployment-specific SQL.
-- Do not grant end-user clients direct table access and claim these procedures are sufficient when
-- @ActorUsername can be fabricated by a shared-identity desktop client.
