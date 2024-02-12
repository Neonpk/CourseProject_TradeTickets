using System;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Discount : ViewModelBase
{
    //Columns

    private System.Int64 _id;
    public System.Int64 Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }

    private string _name;
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }

    private int _discountSize;
    public int DiscountSize { get => _discountSize; set => this.RaiseAndSetIfChanged(ref _discountSize, value); }

    private string _description;
    public string Description { get => _description; set => this.RaiseAndSetIfChanged(ref _description, value); }

    public Discount()
    {
        Name = String.Empty;
        Description = String.Empty;
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