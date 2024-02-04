namespace CourseProject_SellingTickets.Models
{
    // Types 

    public enum FlightSortModes
    {
        FlightNumber = 0,
        DeparturePlace = 1,
        DestinationPlace = 2,
        DepartureTime = 3,
        ArrivalTime = 4,
        AircraftName = 5,
        TotalPlace = 6,
        FreePlace = 7,
        CanceledFlights = 8,
        DurationTime = 9,
        IsCompleted = 10,
        InProgress = 11
    }
    
    public enum FlightSearchModes
    {
        FlightNumber = 0,
        DeparturePlace = 1,
        DestinationPlace = 2,
        DepartureTime = 3,
        ArrivalTime = 4,
        AircraftName = 5,
        TotalPlace = 6,
        FreePlace = 7,
        DurationTime = 8,
    }
    
    public enum SortMode
    {
        Asc = 0,
        Desc = 1
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