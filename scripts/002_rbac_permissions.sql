USE PressDMS;
GO

-- Add normalized, database-maintained role permissions. Safe to re-run.
IF OBJECT_ID(N'dbo.Permissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Permissions
    (
        PermissionID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Permissions PRIMARY KEY,
        PermissionKey NVARCHAR(50) NOT NULL CONSTRAINT UQ_Permissions_PermissionKey UNIQUE,
        Description NVARCHAR(200) NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.RolePermissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RolePermissions
    (
        RoleID INT NOT NULL,
        PermissionID INT NOT NULL,
        CONSTRAINT PK_RolePermissions PRIMARY KEY (RoleID, PermissionID),
        CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleID) REFERENCES dbo.Roles(RoleID),
        CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionID) REFERENCES dbo.Permissions(PermissionID)
    );
END;
GO

INSERT dbo.Permissions (PermissionKey, Description)
SELECT src.PermissionKey, src.Description
FROM (VALUES
    (N'DocumentsRead', N'View and search permitted documents'),
    (N'DocumentsReadAll', N'View documents uploaded by any user'),
    (N'DocumentsWrite', N'Upload documents'),
    (N'AdsRead', N'View advertisements'),
    (N'AdsWrite', N'Create advertisements'),
    (N'PublishingRead', N'View publishing records'),
    (N'PublishingWrite', N'Create publishing records'),
    (N'UsersRead', N'View user accounts'),
    (N'UsersWrite', N'Change user roles and activation state'),
    (N'AuditRead', N'View audit log entries')
) AS src(PermissionKey, Description)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Permissions p WHERE p.PermissionKey = src.PermissionKey);
GO

-- Seed only grants that are part of the documented defaults; preserve any additional grants.
INSERT dbo.RolePermissions (RoleID, PermissionID)
SELECT r.RoleID, p.PermissionID
FROM dbo.Roles r
CROSS JOIN dbo.Permissions p
WHERE r.RoleName = N'Admin'
  AND NOT EXISTS (SELECT 1 FROM dbo.RolePermissions rp WHERE rp.RoleID = r.RoleID AND rp.PermissionID = p.PermissionID);

INSERT dbo.RolePermissions (RoleID, PermissionID)
SELECT r.RoleID, p.PermissionID
FROM dbo.Roles r
JOIN dbo.Permissions p ON p.PermissionKey IN (N'DocumentsRead', N'DocumentsWrite')
WHERE r.RoleName = N'Editor'
  AND NOT EXISTS (SELECT 1 FROM dbo.RolePermissions rp WHERE rp.RoleID = r.RoleID AND rp.PermissionID = p.PermissionID);

INSERT dbo.RolePermissions (RoleID, PermissionID)
SELECT r.RoleID, p.PermissionID
FROM dbo.Roles r
JOIN dbo.Permissions p ON p.PermissionKey IN
    (N'DocumentsRead', N'DocumentsReadAll', N'DocumentsWrite', N'AdsRead', N'AdsWrite', N'PublishingRead', N'PublishingWrite')
WHERE r.RoleName = N'Accountant'
  AND NOT EXISTS (SELECT 1 FROM dbo.RolePermissions rp WHERE rp.RoleID = r.RoleID AND rp.PermissionID = p.PermissionID);
GO

-- Verify expected default roles exist; intentionally do not silently create account identities here.
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = N'Admin')
   OR NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = N'Editor')
   OR NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = N'Accountant')
    THROW 50010, 'Expected application roles are missing. Run the base schema first.', 1;
GO
