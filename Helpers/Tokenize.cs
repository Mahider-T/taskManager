using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
public class Tokenize {
    private const string SecretKey = "TokenizerSecretKey";

    public static string GenerateToken(string userId, string email) {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("userId", userId),
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
        var key = Encoding.UTF8.GetBytes(SecretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        SecurityToken validatedToken;
        return tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
    }


}