using System;

namespace OrderRideService.Commands
{
    public class OrderRideCommand
    {
        public Guid UserId { get; set; }
        public string StartingStreetAddress { get; set; }
        public string StartingCity { get; set; }
        public string StartingState { get; set; }
        public string StartingZipCode { get; set; }
        public string DestinationStreetAddress { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationState { get; set; }
        public string DestinationZipCode { get; set; }
    }
}