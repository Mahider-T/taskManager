using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace TaskManager.Helpers;
public class Tokenize {
    
    static string  secretKey = "Canthisnotbemysecretkeynowitislongenoughpleasesirthisisalot.";

    public static string GenerateToken(string email) {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("email", email)
        };
        var token = new JwtSecurityToken(
            claims : claims,
            expires : DateTime.Now.AddHours(1),
            signingCredentials: credentials
   
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    public static ClaimsPrincipal VerifyToken(string token) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateAudience = false,
            ValidateIssuer = false
        };

        SecurityToken validatedToken;
        return tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
    }


}