using System;
using System.Security.Claims;

namespace ZyzzyvagRPC
{
    internal static class JwtHelper
    {
        public static string GenerateJwtToken(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("Name is not specified.");
            }

            //  var claims = new[] { new Claim(ClaimTypes.Name, name) };
            //   var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            //  var token = new JwtSecurityToken("ExampleServer", "ExampleClients", claims, expires: DateTime.Now.AddSeconds(60), signingCredentials: credentials);
            return "s";//JwtTokenHandler.WriteToken(token);
        }

        // public static readonly JwtSecurityTokenHandler JwtTokenHandler = new JwtSecurityTokenHandler();
        // public static readonly SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());
    }
}