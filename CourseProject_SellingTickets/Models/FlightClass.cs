using CourseProject_SellingTickets.ViewModels;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class FlightClass : ObservableObject
{
    // Main Model 

    private System.Int64? _id;
    public System.Int64? Id { get => _id; set { _id = value; OnPropertyChanged(nameof(Id)); } }

    private string _className;
    public string ClassName { get => _className; set { _className = value; OnPropertyChanged(nameof(ClassName)); } }

    public FlightClass()
    {
        Id = null;
    }
    
    public FlightClass(System.Int64 id, string className)
    {
        Id = id;
        ClassName = className;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is FlightClass o)
        {
            return Id.Equals(o.Id) && 
                   ClassName.Equals(o.ClassName);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}