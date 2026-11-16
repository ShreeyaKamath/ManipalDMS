using System;
using System.Security.Cryptography;

namespace ManipalDMS
{
    /// <summary>PBKDF2-SHA256 password hashing compatible with .NET Framework 4.7.2.</summary>
    public static class PasswordHashing
    {
        public const int SaltSize = 32;
        public const int HashSize = 32;
        public const int Iterations = 310000;

        public static void HashPassword(string password, out byte[] hash, out byte[] salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            salt = new byte[SaltSize];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
                random.GetBytes(salt);

            using (var derive = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                hash = derive.GetBytes(HashSize);
        }

        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrEmpty(password) || storedHash == null || storedSalt == null ||
                storedHash.Length != HashSize || storedSalt.Length != SaltSize)
                return false;

            byte[] computed;
            using (var derive = new Rfc2898DeriveBytes(password, storedSalt, Iterations, HashAlgorithmName.SHA256))
                computed = derive.GetBytes(HashSize);

            int difference = 0;
            for (int i = 0; i < computed.Length; i++)
                difference |= computed[i] ^ storedHash[i];
            return difference == 0;
        }
    }
}
