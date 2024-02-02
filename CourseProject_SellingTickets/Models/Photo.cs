namespace CourseProject_SellingTickets.Models;

public class Photo
{
    
    public System.Int64 Id { get; init; }
    
    public string Name { get; set; }
    
    public string UrlPath { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public Photo( string name, string urlPath, bool isDeleted )
    {
        Name = name;
        UrlPath = urlPath;
        IsDeleted = isDeleted;
    }

    
    public override bool Equals(object? obj)
    {
        if (obj is Photo o)
        {
            return Id.Equals(o.Id) && 
                   Name.Equals(o.Name) && 
                   UrlPath.Equals(o.UrlPath) && 
                   IsDeleted.Equals(o.IsDeleted);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    
    
}