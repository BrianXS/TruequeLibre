using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace API.Utils
{
    public class TokenUtils
    {
        public static string TokenGenerator(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Token.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddMinutes(60);

            return new JwtSecurityTokenHandler()
                .WriteToken(GetToken(Constants.General.AppName, Constants.General.AppName, 
                    claims, expiration, credentials));
        }

        public static string RefreshToken()
        {
            var token = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(token);
            }

            return Convert.ToBase64String(token);
        }

        public static ClaimsPrincipal GetClaims(string oldToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                
                ValidIssuer = Constants.General.AppName,
                ValidAudience = Constants.General.AppName,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Token.Key))
            };
            
            var validationHandler = new JwtSecurityTokenHandler();
            var principal = validationHandler.ValidateToken(oldToken, validationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            
            if(jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                throw new SecurityTokenException();
            
            return principal;
        }
        
        private static JwtSecurityToken GetToken(string issuer, string audience, Claim[] claims, DateTime expirationDate, SigningCredentials credentials)
        {
            return new JwtSecurityToken(issuer: issuer, 
                audience: audience, 
                claims: claims,
                signingCredentials: credentials, 
                expires: expirationDate);
        }
    }
}