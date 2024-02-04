namespace CourseProject_SellingTickets.Models;

public class Aircraft
{
    //Columns
    
    public System.Int64 Id { get; init; }
    
    public string? Model { get; set; }
    
    public string? Type { get; set; }
    
    public int TotalPlace { get; set; }
    
    public Photo Photo { get; set; }

    public Aircraft(System.Int64 id, string? model, string? type, int totalPlace, Photo photo)
    {
        Id = id;
        Model = model;
        Type = type;
        TotalPlace = totalPlace;
        Photo = photo;
    }

    
    public override bool Equals(object? obj)
    {
        
        if (obj is Aircraft o)
        {
            return o.Id.Equals(Id) && 
                   o.Model!.Equals(Model) &&
                   o.Type!.Equals(Type) && 
                   o.TotalPlace.Equals(TotalPlace) &&
                   o.Photo.Equals(Photo);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    
    
}