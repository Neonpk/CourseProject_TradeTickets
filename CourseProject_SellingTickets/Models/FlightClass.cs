namespace CourseProject_SellingTickets.Models;

public class FlightClass
{
    public System.Int64 Id { get; init; }
    public string ClassName { get; set; }

    public FlightClass(System.Int64 id, string className)
    {
        Id = id;
        ClassName = className;
    }

    
    public override bool Equals(object? obj)
    {
        if (obj is FlightClass o)
        {
            return Id.Equals(o.Id) && 
                   ClassName.Equals(o.ClassName);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    
}