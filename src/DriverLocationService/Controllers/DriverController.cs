using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using DriverLocationService.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RandomNameGeneratorLibrary;

namespace DriverLocationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<DriverController> _logger;

        public DriverController(DaprClient daprClient, ILogger<DriverController> logger)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [Topic("messagebus", "ridecreated")]
        [HttpPost("ridecreated")]
        public async Task<ActionResult<DriverAssignedEvent>> GetAvailableDriver(RideCreatedEvent rideCreated)
        {
            var driver = new DriverAssignedEvent(rideCreated.RideId, Guid.NewGuid(), GetDriverName());
            _logger.LogInformation("Assigning driver {DriverName} to Ride: {RideId}", driver.DriverName, driver.RideId);
            await _daprClient.PublishEventAsync("messagebus", "driverassigned", driver);
            return Ok(driver);
        }

        private static string GetDriverName()
        {
            var personGenerator = new PersonNameGenerator();
            return personGenerator.GenerateRandomFirstAndLastName();
        }
    }
}