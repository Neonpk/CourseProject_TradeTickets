namespace CourseProject_SellingTickets.Models
{
    // Types 
    
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