using System;
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
                
                ValidAudience = "truequelibre",
                ValidIssuer = "truequelibre",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Token.Key)),
                
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}