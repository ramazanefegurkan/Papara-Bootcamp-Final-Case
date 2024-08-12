using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Base.Helper.PasswordHasher
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            string saltString = Convert.ToBase64String(salt);
            return $"{saltString}:{hashed}";
        }

        public static bool VerifyPassword(string storedHash, string providedPassword)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }
            var saltString = parts[0];
            var hashString = parts[1];

            byte[] salt = Convert.FromBase64String(saltString);

            string providedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashString == providedHash;
        }
    }
}
