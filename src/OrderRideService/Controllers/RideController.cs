using System;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderRideService.Commands;
using OrderRideService.Events;
using OrderRideService.Models;

namespace OrderRideService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<RideController> _logger;

        public RideController(DaprClient daprClient, ILogger<RideController> logger)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Topic("messagebus", "rideordered")]
        [HttpPost("OrderRide")]
        public async Task<ActionResult<RideCreatedEvent>> OrderRide(OrderRideCommand command)
        {
            _logger.LogInformation("Ride Ordered by {Name}", $"{command.FirstName} {command.LastName}");

            var ride = CreateRideFromCommand(command);
            await _daprClient.SaveStateAsync("default", ride.RideId.ToString(), ride);

            _logger.LogInformation("Saved Ride: {@Ride}", ride);

            var rideCreated = new RideCreatedEvent(
                ride.RideId,
                ride.Leg.Start.StreetAddress,
                ride.Leg.Start.City,
                ride.Leg.Start.State,
                ride.Leg.Start.ZipCode);

            await _daprClient.PublishEventAsync("messagebus", "ridecreated", rideCreated);

            _logger.LogInformation("Sent ride created event for ride: {RideId}", ride.RideId);

            return Ok(rideCreated);
        }

        [Topic("messagebus", "driverassigned")]
        [HttpPut("AssignDriver")]
        public async Task<IActionResult> AssignDriverToRide(AssignDriverCommand command)
        {
            _logger.LogInformation("Driver {DriverName} Assigned to Ride: {RideId}", command.DriverName,
                command.RideId);

            // Get Ride
            var rideStateEntry = await _daprClient.GetStateEntryAsync<Ride>("default", command.RideId.ToString());

            // if ride is new, add it, otherwise update locations
            if (rideStateEntry.Value == null)
            {
                throw new Exception($"Invalid RideId: {command.RideId}");
            }

            rideStateEntry.Value.DriverId = command.DriverId;
            rideStateEntry.Value.DriverName = command.DriverName;
            rideStateEntry.Value.Status = RideStatus.DriverAssigned;

            await rideStateEntry.SaveAsync();
            _logger.LogInformation("Updated Ride: {@Ride}", rideStateEntry.Value);

            return NoContent();
        }

        private static Ride CreateRideFromCommand(OrderRideCommand command)
        {
            return new Ride
            {
                RideId = Guid.NewGuid(),
                User = new User
                {
                    UserId = command.UserId,
                    FirstName = command.FirstName,
                    LastName = command.LastName
                },
                Leg = new Leg
                {
                    Start = new Address
                    {
                        StreetAddress = command.StartingStreetAddress,
                        City = command.StartingCity,
                        State = command.StartingState,
                        ZipCode = command.StartingZipCode
                    },
                    End = new Address
                    {
                        StreetAddress = command.DestinationStreetAddress,
                        City = command.DestinationCity,
                        State = command.DestinationState,
                        ZipCode = command.DestinationZipCode
                    }
                }
            };
        }
    }
}