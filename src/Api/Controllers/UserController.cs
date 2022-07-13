using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Api.Services;
using Api.EFModels;
using System;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : MyBaseController
  {
    private readonly ILogger<UserController> _logger;
    private IUserRepository _userRepository;

    public UserController(ILogger<UserController> logger, IUserRepository userRepository)
    {
      _logger = logger;
      _userRepository = userRepository;
    }

    [HttpGet()]
    public IActionResult Get(string guid)
    {
      try
      {
        var entity = _userRepository.Get(guid);
        if (entity == null)
          return NotFound();

        var response = new UserResponse();
        response.guid = entity.guid;
        response.username = entity.username;
        response.email = entity.email;

        return Ok(response);
      }
      catch (Exception ex)
      {
        var msg = "Error retrieving data from the database";
        _logger.LogError(0, ex, "UserGuid:" + UserGuid + " : " + msg);
        return StatusCode(StatusCodes.Status500InternalServerError, msg);
      }
    }

    [HttpPost()]
    public IActionResult Create([FromBody] UserRequest dto)
    {
      if (dto == null)
        return BadRequest();

      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      //TODO: Valid if exist

      try
      {
        var entity = new User();
        entity.guid = System.Guid.NewGuid().ToString();
        entity.username = dto.username;
        entity.password = dto.password;
        entity.email = dto.email;
        _userRepository.Create(entity);

        var response = new UserResponse();
        response.guid = entity.guid;
        response.username = dto.username;
        response.email = dto.email;

        return CreatedAtAction(nameof(Get), new { guid = response.guid }, response);
      }
      catch (Exception ex)
      {
        var msg = "Error creating new user";
        _logger.LogError(0, ex, "UserGuid:" + UserGuid + " : " + msg);
        return StatusCode(StatusCodes.Status500InternalServerError, msg);
      }
    }
  }
}
