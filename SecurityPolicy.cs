using System;
using System.Data;
using System.Data.SqlClient;

namespace ManipalDMS
{
    public enum UserRole { Admin, Editor, Accountant }

    public enum Permission
    {
        DocumentsRead,
        DocumentsWrite,
        AdsRead,
        AdsWrite,
        PublishingRead,
        PublishingWrite,
        UsersRead,
        UsersWrite,
        AuditRead
    }

    public static class SecurityPolicy
    {
        public static bool HasRole(string role, UserRole expected)
        {
            return !string.IsNullOrWhiteSpace(role)
                && string.Equals(role.Trim(), expected.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        // Permissions are loaded from SQL so role grants can be administered without changing the app.
        // Fail closed if the permission schema is missing or the database cannot be reached.
        public static bool HasPermission(string role, Permission permission)
        {
            if (string.IsNullOrWhiteSpace(role)) return false;

            try
            {
                DataTable result = DatabaseHelper.ExecuteQuery(
                    @"SELECT TOP (1) 1
                      FROM dbo.Roles r
                      INNER JOIN dbo.RolePermissions rp ON rp.RoleID = r.RoleID
                      INNER JOIN dbo.Permissions p ON p.PermissionID = rp.PermissionID
                      WHERE r.RoleName = @RoleName AND p.PermissionKey = @PermissionKey",
                    new SqlParameter("@RoleName", SqlDbType.NVarChar, 30) { Value = role.Trim() },
                    new SqlParameter("@PermissionKey", SqlDbType.NVarChar, 50) { Value = permission.ToString() });
                return result.Rows.Count == 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Permission lookup failed closed: {0}", ex);
                return false;
            }
        }

        public static bool IsAdmin(string role) { return HasRole(role, UserRole.Admin); }
        public static bool IsEditor(string role) { return HasRole(role, UserRole.Editor); }
        public static bool IsAccountant(string role) { return HasRole(role, UserRole.Accountant); }
        public static bool CanManageDocuments(string role) { return HasPermission(role, Permission.DocumentsRead) && HasPermission(role, Permission.DocumentsWrite); }
        public static bool CanManageAds(string role) { return HasPermission(role, Permission.AdsRead) && HasPermission(role, Permission.AdsWrite); }
        public static bool CanManagePublishing(string role) { return HasPermission(role, Permission.PublishingRead) && HasPermission(role, Permission.PublishingWrite); }
        public static bool CanManageUsers(string role) { return HasPermission(role, Permission.UsersRead) && HasPermission(role, Permission.UsersWrite); }
        public static bool CanViewAuditLog(string role) { return HasPermission(role, Permission.AuditRead); }

        public static bool CanViewAllDocuments(string role)
        {
            return IsAdmin(role) || IsAccountant(role);
        }
    }
}
