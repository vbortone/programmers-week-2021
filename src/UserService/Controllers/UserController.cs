using System;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RandomNameGeneratorLibrary;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
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
        
        [HttpGet("/:id:guid")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _daprClient.GetStateEntryAsync<User>("default", id.ToString());
            if (user.Value == null)
            {
                user.Value = CreateNewUser(id);
                await user.SaveAsync();
            }

            return Ok(user.Value);
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