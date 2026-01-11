using System;
using CourseProject_SellingTickets.ValidationRules;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Airline : ReactiveObject, IValidatableViewModel
{

    // Main Model 
    
    private System.Int64 _id;
    public System.Int64 Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }
    
    private string _name;
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }

    // Validation 
    
    private string _errorValidations;
    public string ErrorValidations { get => _errorValidations; set => this.RaiseAndSetIfChanged(ref _errorValidations, value); }
    
    public ValidationContext ValidationContext { get; } = new ValidationContext();
    
    public Airline()
    {
        Name = String.Empty;
        
        this.InitializeValidationRules();
    }
    
    public Airline(System.Int64 id, string name)
    {
        Id = id;
        Name = name;
        
        this.InitializeValidationRules();
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is Airline o)
        {
            return !Nullable.Equals(o, null) &&  
                   o.Id.Equals(Id) && 
                   o.Name!.Equals(Name);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}