using System;

namespace OrderRideService.Events
{
    public class RideCreatedEvent
    {
        public RideCreatedEvent(Guid rideId, string streetAddress, string city, string state, string zipCode)
        {
            RideId = rideId;
            StreetAddress = streetAddress;
            City = city;
            State = state;
            ZipCode = zipCode;
        }
        public Guid RideId { get; }
        public string StreetAddress { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
    }
}