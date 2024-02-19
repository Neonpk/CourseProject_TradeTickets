using System.Runtime.Serialization;

namespace CourseProject_SellingTickets.Models
{
    // Types 

    public enum PostgresStates
    {
        [EnumMember(Value = "23505")]
        UniqueViolation
    }
    
    public enum FlightSortModes
    {
        FlightNumber = 0,
        DeparturePlace,
        DestinationPlace,
        DepartureTime,
        ArrivalTime,
        AircraftName,
        TotalPlace,
        FreePlace,
        CanceledFlights,
        DurationTime,
        IsCompleted,
        InProgress
    }
    
    public enum FlightSearchModes
    {
        FlightNumber = 0,
        DeparturePlace,
        DestinationPlace,
        DepartureTime,
        ArrivalTime,
        AircraftName,
        TotalPlace,
        FreePlace,
        DurationTime,
    }

    public enum TicketSearchModes
    {
        TicketNumber = 0,
        FlightNumber,
        DeparturePlace,
        DestinationPlace,
        FlightClass,
        PlaceNumber,
        DiscountType,
        Price,
        DiscountPrice,
        DepartureTime,
        ArrivalTime
    }
    
    public enum SortMode
    {
        Asc = 0,
        Desc
    }
    
    public enum OperatingModes
    {
        AdminMode = 0,
        DispatcherMode
    }

    public enum ConnectionStates
    {
        Connected = 0,
        Disconnected,
        TimedOut
    }

    public enum AuthStates
    {
        None = 0,
        Success,
        Failed
    }
    
}