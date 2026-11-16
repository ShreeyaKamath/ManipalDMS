using System;

namespace ManipalDMS
{
    public static class SecurityPolicy
    {
        public static bool IsAdmin(string role)
        {
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsEditor(string role)
        {
            return string.Equals(role, "Editor", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsAccountant(string role)
        {
            return string.Equals(role, "Accountant", StringComparison.OrdinalIgnoreCase);
        }

        public static bool CanManageDocuments(string role)
        {
            return IsAdmin(role) || IsEditor(role) || IsAccountant(role);
        }

        public static bool CanManageAds(string role)
        {
            return IsAdmin(role);
        }

        public static bool CanManagePublishing(string role)
        {
            return IsAdmin(role);
        }
    }
}
