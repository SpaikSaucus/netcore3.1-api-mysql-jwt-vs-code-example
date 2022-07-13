using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Api.Helper;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Api.Middlewares
{
  public class TokenMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TokenMiddleware> _logger;

    public TokenMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<TokenMiddleware> logger)
    {
      this._next = next;
      this._configuration = configuration;
      this._logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
      if (!context.Request.Path.Value.StartsWith("/api/auth") && context.Request.Method != HttpMethods.Options.ToString())
      {
        var token = context.Request.GetHeader("Authorization");
        var secretKey = _configuration.GetValue<string>("SecretKey");
        var secretKeyBytes = Encoding.ASCII.GetBytes(secretKey);

        // TODO: Valid expiration token.
        // TODO: Valid ip from generate token.

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidator = new TokenValidationParameters()
        {
          ValidateLifetime = false, // Because there is no expiration in the generated token
          ValidateAudience = false, // Because there is no audiance in the generated token
          ValidateIssuer = false,   // Because there is no issuer in the generated token
          ValidIssuer = "Sample",
          ValidAudience = "Sample",
          IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes) // The same key as the one that generate the token
        };

        SecurityToken validatedToken;
        try
        {
          var principal = tokenHandler.ValidateToken(token, tokenValidator, out validatedToken);

          context.Items["userGuid"] = principal.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        }
        catch (System.Exception ex)
        {
          var msg = "Authentication required to execute this request";
          _logger.LogError(0, ex, msg);

          context.Response.StatusCode = 401;
          context.Response.ContentType = "text/plain";
          await context.Response.WriteAsync(msg);

          return;
        }
      }

      await this._next(context);
    }
  }

  public static class TokenMiddlewareExtensions
  {
    public static IApplicationBuilder UseToken(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<TokenMiddleware>();
    }
  }
}
