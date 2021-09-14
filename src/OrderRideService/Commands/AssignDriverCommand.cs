using System;

namespace OrderRideService.Commands
{
    public class AssignDriverCommand
    {
        public Guid RideId { get; set; }
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }
    }
}