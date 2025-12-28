using System.Runtime.Serialization;

namespace CourseProject_SellingTickets.Models
{
    // Types 

    public enum PostgresStates
    {
        [EnumMember(Value = "23505")]
        UniqueViolation
    }
    
    public enum AircraftSearchSortModes
    {
        Model = 0,
        Type,
        TotalPlace
    }

    public enum PlaceSearchSortModes
    {
        Name = 0,
        Description,
    }

    public enum DiscountSearchSortModes
    {
        Name = 0,
        Description,
        DiscountSize
    }

    public enum PhotoSearchSortModes
    {
        Name = 0,
        UrlPath, 
    }

    public enum FlightClassSearchSortModes
    {
        ClassName = 0
    }

    public enum AirlineSearchSortModes
    {
        Name = 0,
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
        InProgress,
        Price
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
        Price,
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
        ArrivalTime,
        UserCustomer
    }
    
    public enum SortMode
    {
        Asc = 0,
        Desc
    }
    
    public enum UserRoles
    {
        Admin = 0,
        Dispatcher,
        User
    }

    public enum AuthStates
    {
        None = 0,
        Success,
        Failed
    }
}
