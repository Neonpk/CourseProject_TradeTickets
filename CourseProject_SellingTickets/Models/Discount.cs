using CourseProject_SellingTickets.ViewModels;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Discount : ObservableObject
{
    //Columns

    private System.Int64? _id;
    public System.Int64? Id { get => _id; set { _id = value; OnPropertyChanged(nameof(Id)); } }

    private string _name;
    public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

    private int _discountSize;
    public int DiscountSize { get => _discountSize; set { _discountSize = value; OnPropertyChanged(nameof(DiscountSize)); } }

    private string _description;
    public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }

    public Discount()
    {
        Id = null;
    }
    
    public Discount(System.Int64 id, string name, int discountSize, string description)
    {
        Id = id;
        Name = name;
        DiscountSize = discountSize;
        Description = description;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is Discount o)
        {
            return Id.Equals(o.Id) && 
                   Name.Equals(o.Name) &&
                   DiscountSize.Equals(o.DiscountSize) && 
                   Description.Equals(o.Description);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}