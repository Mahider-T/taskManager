using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using TaskManager.Helpers;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _cookieName;
    private readonly string _secretKey;

    public TokenValidationMiddleware(RequestDelegate next, string cookieName, string secretKey)
    {
        _next = next;
        _cookieName = cookieName;
        _secretKey = secretKey;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue(_cookieName, out var token))
        {
            Console.WriteLine(_cookieName);
            try
            {
                // Console.WriteLine("Your toeken is 1" + token + "1");
                var principal = Tokenize.VerifyToken(token);
                // Console.WriteLine(principal == null);

                if (principal != null)
                {
                    // Console.WriteLine("Context.User is set");
                    context.User = principal;
                }
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenValidationException ex)
            {
                // Handle token validation failure (e.g., log the error)
                Console.WriteLine($"Token validation failed: {ex.Message}");
            }
        }

        await _next(context);
    }
}
