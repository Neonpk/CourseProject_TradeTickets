using System;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Airline : ReactiveObject
{

    // Main Model 
    
    private System.Int64 _id;
    public System.Int64 Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }
    
    private string _name;
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }

    public Airline()
    {
        Name = String.Empty;
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