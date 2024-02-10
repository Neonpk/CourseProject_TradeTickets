using CourseProject_SellingTickets.ViewModels;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Airline : ObservableObject
{

    // Main Model 
    
    private System.Int64? _id;
    public System.Int64? Id { get => _id; set { _id = value; OnPropertyChanged(nameof(Id)); } }
    
    private string _name;
    public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

    public Airline()
    {
        Id = null;
    }
    
    public Airline(System.Int64 id, string name)
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