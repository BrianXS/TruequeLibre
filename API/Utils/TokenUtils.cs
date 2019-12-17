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
            var expiration = DateTime.Now.AddDays(1);

            return new JwtSecurityTokenHandler()
                .WriteToken(GetToken("truequelibre", "truequelibre", 
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