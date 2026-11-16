using System;
using System.Security.Cryptography;

namespace ManipalDMS
{
    public static class PasswordHashing
    {
        public static (byte[] Hash, byte[] Salt) HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            var salt = RandomNumberGenerator.GetBytes(32);

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                200000,
                HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                return (hash, salt);
            }
        }

        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                storedSalt,
                200000,
                HashAlgorithmName.SHA256))
            {
                byte[] computedHash = pbkdf2.GetBytes(32);
                return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
            }
        }
    }
}
