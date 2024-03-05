using System;
using CourseProject_SellingTickets.ValidationRules;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace CourseProject_SellingTickets.Models;

#pragma warning  disable
public class Place : ReactiveObject, IValidatableViewModel
{
    //Columns 

    private System.Int64 _id;
    public System.Int64 Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }

    private string _name;
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }

    private string _description;
    public string Description { get => _description; set => this.RaiseAndSetIfChanged(ref _description, value); }

    private Photo _photo;
    public Photo Photo { get => _photo; set => this.RaiseAndSetIfChanged(ref _photo, value); }

    // Validation 
    
    private string _errorValidations;
    public string ErrorValidations { get => _errorValidations; set => this.RaiseAndSetIfChanged(ref _errorValidations, value); }
    
    public ValidationContext ValidationContext { get; } = new ValidationContext();
    
    public Place()
    {
        Name = string.Empty;
        Description = string.Empty;
        Photo = new Photo();
        
        this.InitializeValidationRules();
    }
    
    public Place( long id, string name, string description, Photo photo  )
    {
        Id = id;
        Name = name;
        Description = description;
        Photo = photo;
        
        this.InitializeValidationRules();
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is Place o)
        {
            return Id.Equals(o.Id) && 
                   Name.Equals(o.Name) && 
                   Description.Equals(o.Description) &&
                   Photo.Equals(o.Photo);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}