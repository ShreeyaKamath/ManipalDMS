using System;

namespace ManipalDMS
{
    public enum UserRole
    {
        Admin,
        Editor,
        Accountant
    }

    public enum Permission
    {
        DocumentsRead,
        DocumentsWrite,
        AdsRead,
        AdsWrite,
        PublishingRead,
        PublishingWrite,
        UsersRead,
        UsersWrite
    }

    public static class SecurityPolicy
    {
        public static bool HasRole(string role, UserRole expected)
        {
            if (string.IsNullOrWhiteSpace(role))
                return false;

            return string.Equals(role.Trim(), expected.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasPermission(string role, Permission permission)
        {
            if (string.IsNullOrWhiteSpace(role))
                return false;

            switch (permission)
            {
                case Permission.DocumentsRead:
                case Permission.DocumentsWrite:
                    return HasRole(role, UserRole.Admin)
                        || HasRole(role, UserRole.Editor)
                        || HasRole(role, UserRole.Accountant);

                case Permission.AdsRead:
                case Permission.AdsWrite:
                    return HasRole(role, UserRole.Admin)
                        || HasRole(role, UserRole.Accountant);

                case Permission.PublishingRead:
                case Permission.PublishingWrite:
                    return HasRole(role, UserRole.Admin)
                        || HasRole(role, UserRole.Accountant);

                case Permission.UsersRead:
                case Permission.UsersWrite:
                    return HasRole(role, UserRole.Admin);

                default:
                    return false;
            }
        }

        public static bool IsAdmin(string role) { return HasRole(role, UserRole.Admin); }
        public static bool IsEditor(string role) { return HasRole(role, UserRole.Editor); }
        public static bool IsAccountant(string role) { return HasRole(role, UserRole.Accountant); }

        public static bool CanManageDocuments(string role)
        {
            return HasPermission(role, Permission.DocumentsRead)
                && HasPermission(role, Permission.DocumentsWrite);
        }

        public static bool CanManageAds(string role)
        {
            return HasPermission(role, Permission.AdsRead)
                && HasPermission(role, Permission.AdsWrite);
        }

        public static bool CanManagePublishing(string role)
        {
            return HasPermission(role, Permission.PublishingRead)
                && HasPermission(role, Permission.PublishingWrite);
        }

        public static bool CanManageUsers(string role)
        {
            return HasPermission(role, Permission.UsersRead)
                && HasPermission(role, Permission.UsersWrite);
        }
    }
}
