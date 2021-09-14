using System;

namespace OrderRideService.Models
{
    public class Ride
    {
        public Ride()
        {
            Status = RideStatus.Created;
        }
        
        public Guid RideId { get; set; }
        public RideStatus Status { get; set; }
        public User User { get; set; }
        public Leg Leg { get; set; }
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }
    }
}