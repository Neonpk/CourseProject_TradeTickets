namespace CourseProject_SellingTickets.Models;

public class Airline
{
    public System.Int64 Id { get; init;  }
    public string? Name { get; set; }

    public Airline(System.Int64 id, string? name)
    {
        Id = id;
        Name = name;
    }

    
    public override bool Equals(object? obj)
    {
        if (obj is Airline o)
        {
            return o.Id.Equals(Id) && 
                   o.Name!.Equals(Name);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    
    
}