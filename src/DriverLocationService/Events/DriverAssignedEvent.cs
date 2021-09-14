using System;

namespace DriverLocationService.Events
{
    public class DriverAssignedEvent
    {
        public DriverAssignedEvent(Guid rideId, Guid driverId, string driverName)
        {
            RideId = rideId;
            DriverId = driverId;
            DriverName = driverName;
        }
        public Guid RideId { get; set; }
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }
    }
}