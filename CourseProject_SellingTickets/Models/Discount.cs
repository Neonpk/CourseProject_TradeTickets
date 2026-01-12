using System;
using CourseProject_SellingTickets.ValidationRules;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Discount : ReactiveObject, IValidatableViewModel
{
    // Guid (Нужен для идентификации объектов сравнения)

    public Guid Guid { get; } = Guid.NewGuid();
    
    //Columns

    private System.Int64 _id;
    public System.Int64 Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }

    private string _name;
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }

    private int _discountSize;
    public int DiscountSize { get => _discountSize; set => this.RaiseAndSetIfChanged(ref _discountSize, value); }

    private string _description;
    public string Description { get => _description; set => this.RaiseAndSetIfChanged(ref _description, value); }

    // Validation 
    
    private string _errorValidations;
    public string ErrorValidations { get => _errorValidations; set => this.RaiseAndSetIfChanged(ref _errorValidations, value); }
    
    public ValidationContext ValidationContext { get; } = new ValidationContext();
    
    public Discount()
    {
        Name = String.Empty;
        Description = String.Empty;
        
        this.InitializeValidationRules();
    }
    
    public Discount(Int64 id, string name, int discountSize, string description)
    {
        Id = id;
        Name = name;
        DiscountSize = discountSize;
        Description = description;
        
        this.InitializeValidationRules();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Discount discount && Id.Equals(discount.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
