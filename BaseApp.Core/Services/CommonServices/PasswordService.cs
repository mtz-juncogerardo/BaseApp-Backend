using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BaseApp.Core.Helpers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BaseApp.Core.Services.CommonServices
{
    public static class PasswordService
    {
        public static string GetHashPassword(string password, string salt)
        {
            if (!ValidatePassword(password))
            {
                CustomException.Throw("La contraseña no cumple con los parametros de seguridad", 400);
            }
            var strongPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                Encoding.ASCII.GetBytes(salt),
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));

            return strongPassword;
        }

        public static void ValidatePasswordMatching(string password, string salt, string hashedPassword)
        {
            var hashToCompare = GetHashPassword(password, salt);
            if (hashToCompare != hashedPassword)
            {
                CustomException.Throw("El usuario o la contraseña son incorrectos", 401);   
            }
        }
        
        public static string GenerateSalt()
        {
            var byteSalt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(byteSalt);
            }
            var stringSalt = Convert.ToBase64String(byteSalt);

            return stringSalt;
        }
        
        private static bool ValidatePassword(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsNumber) &&
                   password.Any(char.IsPunctuation);
        }
    }
}