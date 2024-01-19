namespace CourseProject_SellingTickets.Models;

public class FlightClass
{
    public System.Int64 Id { get; }
    public string ClassName { get; }

    public FlightClass(System.Int64 id, string className)
    {
        Id = id;
        ClassName = className;
    }
    
}