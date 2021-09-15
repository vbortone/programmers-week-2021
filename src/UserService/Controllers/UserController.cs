using System;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RandomNameGeneratorLibrary;
using UserService.Models;

namespace UserService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly DaprClient _daprClient;
    private readonly ILogger<UserController> _logger;

    public UserController(DaprClient daprClient, ILogger<UserController> logger)
    {
      _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<ActionResult<User>> Get([FromQuery] Guid userId)
    {
      var userState = await _daprClient.GetStateEntryAsync<User>("default", userId.ToString());
      if (userState.Value == null)
      {
        userState.Value = CreateNewUser(userId);
        await userState.SaveAsync();
      }
      _logger.LogInformation("User Loaded or Created: {FirstName} {LastName} ({UserId})", userState.Value.FirstName, userState.Value.LastName, userState.Value.UserId);

      return Ok(userState.Value);
    }

    private static User CreateNewUser(Guid id)
    {
      var personGenerator = new PersonNameGenerator();
      var user = new User
      {
        UserId = id,
        FirstName = personGenerator.GenerateRandomFirstName(),
        LastName = personGenerator.GenerateRandomLastName()
      };
      return user;
    }

  }
}