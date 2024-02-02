namespace CourseProject_SellingTickets.Models;

public class Place
{
    
    public System.Int64 Id { get; init; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public Photo Photo { get; set; }

    public Place( System.Int64 id, string name, string description, Photo photo  )
    {
        Id = id;
        Name = name;
        Description = description;
        Photo = photo;
    }

    
    public override bool Equals(object? obj)
    {
        if (obj is Place o)
        {
            return Id.Equals(o.Id) && 
                   Name.Equals(o.Name) && 
                   Description.Equals(o.Description) &&
                   Photo.Equals(o.Photo);

        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    
}