namespace CourseProject_SellingTickets.Models;

public class Photo
{
    public string Name { get; }
    
    public string UrlPath { get; }
    
    public bool IsDeleted { get; }
    
    public Photo( string name, string urlPath, bool isDeleted )
    {
        Name = Name;
        UrlPath = urlPath;
        IsDeleted = isDeleted;
    }
    
}