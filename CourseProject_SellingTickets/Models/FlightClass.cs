using System;
using CourseProject_SellingTickets.ValidationRules;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class FlightClass : ReactiveObject, IValidatableViewModel
{
    // Guid (Нужен для идентификации объектов сравнения)

    public Guid Guid { get; } = Guid.NewGuid();
    
    // Main Model 

    private Int64 _id;
    public Int64 Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }

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
    
    public FlightClass(Int64 id, string className)
    {
        Id = id;
        ClassName = className;
        
        this.InitializeValidationRules();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is FlightClass flightClass && Id.Equals(flightClass.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
