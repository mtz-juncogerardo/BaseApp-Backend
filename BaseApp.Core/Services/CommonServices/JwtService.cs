using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using BaseApp.Core.Helpers;
using BaseApp.Data;
using BaseApp.Data.DbModels;
using Microsoft.IdentityModel.Tokens;

namespace BaseApp.Core.Services.CommonServices
{
    public static class JwtService
    {
        private const string Url = "https://mtzjunco.com";

        public static string GenerateTokenWithClaims(string secret, DateTime expirationDate, IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var loginCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);
            var tokenOptions = new JwtSecurityToken(
                Url,
                Url,
                claims,
                expires: expirationDate,
                signingCredentials: loginCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

        public static string GenerateTokenWithUserId(string secret,
            DateTime expirationDate,
            string userId,
            string versionId,
            UserRoleEnum role = UserRoleEnum.User)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var loginCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);
            var tokenOptions = new JwtSecurityToken(
                Url,
                Url,
                new List<Claim>
                {
                    new("UserId", userId),
                    new("VersionId", versionId),
                    new("UserRole", role.ToString())
                },
                expires: expirationDate,
                signingCredentials: loginCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

        public static string GetClaimUserId(IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity!;
            ValidateToken(claimsIdentity);
            var userId = claimsIdentity!.Claims.FirstOrDefault(r => r.Type == "UserId");
            return userId!.Value;
        }

        public static IEnumerable<Claim> GetClaims(IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity!;
            ValidateToken(claimsIdentity);
            return claimsIdentity.Claims.ToList();
        }

        private static void ValidateToken(object? claim)
        {
            if (claim == null)
            {
                CustomException.Throw("No se pudo validar la identidad en el token", 401);
            }
        }

        public static void ValidateJwtVersion(AuthenticationDbModel auth, IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity!;
            var claimVersionId = claimsIdentity!.Claims.FirstOrDefault(r => r.Type == "VersionId");
            ValidateToken(claimVersionId);
            if (auth.VersionId != claimVersionId!.Value)
            {
                CustomException.Throw("No se pudo validar la autenticidad del token", 401);
            }
        }
    }
}