using System;

namespace DriverLocationService.Events
{
    public class RideCreatedEvent
    {
        public Guid RideId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}