using System;
using CourseProject_SellingTickets.ValidationRules;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Ticket : ReactiveObject, IValidatableViewModel
{
    // Guid (Нужен для идентификации объектов сравнения)

    public Guid Guid { get; } = Guid.NewGuid();
    
    // Columns

    private Int64 _id;
    public Int64 Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }

    private Flight _flight;
    public Flight Flight { get => _flight; set => this.RaiseAndSetIfChanged(ref _flight, value); }

    private FlightClass _flightClass;
    public FlightClass FlightClass { get => _flightClass; set => this.RaiseAndSetIfChanged(ref _flightClass, value); }

    private int _placeNumber;
    public int PlaceNumber { get => _placeNumber; set => this.RaiseAndSetIfChanged(ref _placeNumber, value); }

    private Discount _discount;
    public Discount Discount { get => _discount; set => this.RaiseAndSetIfChanged(ref _discount, value); }

    private bool _isSold;
    public bool IsSold { get => _isSold; set => this.RaiseAndSetIfChanged(ref _isSold, value); }

    private User? _user;
    public User? User { get => _user; set => this.RaiseAndSetIfChanged(ref _user, value); }

    private int _price;

    // Non observable 
    
    public int Price => Flight.Price;
    public double DiscountPrice => Price - Price * (Discount.DiscountSize * 0.01);
    
    // Validations 
        
    private string _errorValidations;
    public string ErrorValidations { get => _errorValidations; set => this.RaiseAndSetIfChanged(ref _errorValidations, value); }
    public ValidationContext ValidationContext { get; } = new ValidationContext();
    
    public Ticket()
    {
        Flight = new Flight();
        FlightClass = new FlightClass();
        Discount = new Discount();
        User = new User();
        
        // Validations 
        
        this.InitializeValidationRules();
    }
    
    public Ticket( long id, Flight flight, FlightClass flightClass, 
        int placeNumber, Discount discount, bool isSold, User? user )
    {
        Id = id;
        Flight = flight;
        FlightClass = flightClass;
        PlaceNumber = placeNumber;
        Discount = discount;
        IsSold = isSold;
        User = user;
        
        // Validations 
        
        this.InitializeValidationRules();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Ticket ticket && Id.Equals(ticket.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
