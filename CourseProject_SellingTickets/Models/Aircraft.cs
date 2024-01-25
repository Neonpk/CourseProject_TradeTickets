namespace CourseProject_SellingTickets.Models;

public class Aircraft
{
    //Columns
    
    public System.Int64 Id { get; }
    
    public string? Model { get; }
    
    public string? Type { get; }
    
    public int TotalPlace { get; }
    
    public Photo Photo { get; }

    public Aircraft(System.Int64 id, string? model, string? type, int totalPlace, Photo photo)
    {
        Id = id;
        Model = model;
        Type = type;
        TotalPlace = totalPlace;
        Photo = photo;
    }
    
}