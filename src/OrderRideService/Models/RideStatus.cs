namespace OrderRideService.Models
{
    public enum RideStatus
    {
        Created,
        DriverAssigned,
        DriverDispatched,
        RideStarted,
        DestinationReached,
        RideSettled,
        DriverReviewed,
        RideCompleted
    }
}