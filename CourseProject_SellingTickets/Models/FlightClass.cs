using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class FlightClass : ViewModelBase
{
    // Main Model 

    private System.Int64? _id;
    public System.Int64? Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }

    private string _className;
    public string ClassName { get => _className; set => this.RaiseAndSetIfChanged(ref _className, value); }

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