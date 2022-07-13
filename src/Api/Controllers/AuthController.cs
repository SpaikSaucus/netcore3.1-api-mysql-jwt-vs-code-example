using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Api.Services;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Security.Claims;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private IUserRepository _userRepository;

    public AuthController(IConfiguration configuration, ILogger<AuthController> logger, IUserRepository userRepository)
    {
      _configuration = configuration;
      _logger = logger;
      _userRepository = userRepository;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("[action]")]
    public IActionResult Login(LoginRequest model)
    {
      try
      {

        var entity = _userRepository.GetByUsername(model.username);
        if (entity == null)
          return NoContent();
        else if (entity.password != model.password)
          return Unauthorized();

        //TODO: Serialize password before store DB and deserialize here
        //isn't same -> return Unauthorized();

        var expirationHours = _configuration.GetValue<uint>("ExpirationTokenHours");
        var secretKey = _configuration.GetValue<string>("SecretKey");
        var key = Encoding.ASCII.GetBytes(secretKey);

        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, entity.username));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, entity.guid.ToString()));
        claims.AddClaim(new Claim(ClaimTypes.Email, entity.email));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = claims,
          Expires = DateTime.UtcNow.AddHours(expirationHours),
          SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var createdToken = tokenHandler.CreateToken(tokenDescriptor);

        object someObject = new
        {
          token = tokenHandler.WriteToken(createdToken)
        };

        return Ok(someObject);

      }
      catch (Exception ex)
      {
        var msg = "Error login validations";
        _logger.LogError(0, ex, msg);
        return StatusCode(StatusCodes.Status500InternalServerError, msg);
      }
    }
  }
}
