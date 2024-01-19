namespace CourseProject_SellingTickets.Models;

public class Airline
{

    public System.Int64 Id { get; }
    public string Name { get;  }
    
    public Airline(System.Int64 id, string name)
    {
        Id = id;
        Name = name;
    }
    
}