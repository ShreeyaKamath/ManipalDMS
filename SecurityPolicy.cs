using System;

namespace ManipalDMS
{
    /// <summary>Application navigation policy. This is not a security boundary; enforce the same policy in a trusted service/database.</summary>
    public static class SecurityPolicy
    {
        public static bool IsAdmin(string role) { return HasRole(role, "Admin"); }
        public static bool IsEditor(string role) { return HasRole(role, "Editor"); }
        public static bool IsAccountant(string role) { return HasRole(role, "Accountant"); }

        public static bool CanManageDocuments(string role)
        {
            return IsAdmin(role) || IsEditor(role) || IsAccountant(role);
        }

        public static bool CanManageAds(string role)
        {
            return IsAdmin(role) || IsAccountant(role);
        }

        public static bool CanManagePublishing(string role)
        {
            return IsAdmin(role) || IsAccountant(role);
        }

        private static bool HasRole(string actual, string expected)
        {
            return string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase);
        }
    }
}
