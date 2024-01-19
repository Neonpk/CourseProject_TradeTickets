namespace CourseProject_SellingTickets.Models;

public class Place
{
    
    public System.Int64 Id { get; }
    
    public string Name { get; }
    
    public string Description { get; }
    
    public Photo Photo { get; }

    public Place( System.Int64 id, string name, string description, Photo photo  )
    {
        Id = id;
        Name = name;
        Description = description;
        Photo = photo;
    }
    
}