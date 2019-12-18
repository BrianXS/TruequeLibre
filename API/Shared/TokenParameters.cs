using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace API.Shared
{
    public class TokenParameters
    {
        public static TokenValidationParameters GetParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                
                ValidAudience = Constants.General.AppName,
                ValidIssuer = Constants.General.AppName,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Token.Key)),
                
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}