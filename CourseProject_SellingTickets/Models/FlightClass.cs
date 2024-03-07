using System;
using CourseProject_SellingTickets.ValidationRules;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class FlightClass : ReactiveObject, IValidatableViewModel
{
    // Main Model 

    private System.Int64 _id;
    public System.Int64 Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }

    private string _className;
    public string ClassName { get => _className; set => this.RaiseAndSetIfChanged(ref _className, value); }

    // Validation
    
    private string _errorValidations;
    public string ErrorValidations { get => _errorValidations; set => this.RaiseAndSetIfChanged(ref _errorValidations, value); }
    
    public ValidationContext ValidationContext { get; } = new ValidationContext();
    
    public FlightClass()
    {
        ClassName = String.Empty;
        
        this.InitializeValidationRules();
    }
    
    public FlightClass(System.Int64 id, string className)
    {
        Id = id;
        ClassName = className;
        
        this.InitializeValidationRules();
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