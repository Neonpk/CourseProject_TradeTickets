using CourseProject_SellingTickets.ViewModels;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Photo : ObservableObject
{
    // Main Model

    private System.Int64? _id;
    public System.Int64? Id { get => _id; set { _id = value; OnPropertyChanged(nameof(Id)); } }

    private string _name;
    public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

    private string _urlPath;
    public string UrlPath { get => _urlPath; set { _urlPath = value; OnPropertyChanged(nameof(UrlPath)); } }

    private bool _isDeleted;
    public bool IsDeleted { get => _isDeleted; set { _isDeleted = value; OnPropertyChanged(nameof(IsDeleted)); } }

    public Photo()
    {
        Id = null;
    }
    
    public Photo(string name, string urlPath, bool isDeleted)
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