using CourseProject_SellingTickets.ValidationRules;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Ticket : ReactiveObject, IValidatableViewModel
{
    // Columns

    private System.Int64 _id;
    public System.Int64 Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }

    private Flight _flight;
    public Flight Flight { get => _flight; set => this.RaiseAndSetIfChanged(ref _flight, value); }

    private FlightClass _flightClass;
    public FlightClass FlightClass { get => _flightClass; set => this.RaiseAndSetIfChanged(ref _flightClass, value); }

    private int _placeNumber;
    public int PlaceNumber { get => _placeNumber; set => this.RaiseAndSetIfChanged(ref _placeNumber, value); }

    private int _price;
    public int Price { get => _price; set => this.RaiseAndSetIfChanged(ref _price, value); }

    private Discount _discount;
    public Discount Discount { get => _discount; set => this.RaiseAndSetIfChanged(ref _discount, value); }

    private bool _isSold;
    public bool IsSold { get => _isSold; set => this.RaiseAndSetIfChanged(ref _isSold, value); }
    
    // Non Observable
    public float DiscountPrice { get; }

    // Validations 
        
    private string _errorValidations;
    public string ErrorValidations { get => _errorValidations; set => this.RaiseAndSetIfChanged(ref _errorValidations, value); }
    public ValidationContext ValidationContext { get; } = new ValidationContext();
    
    public Ticket()
    {
        Flight = new Flight();
        FlightClass = new FlightClass();
        Discount = new Discount();
        
        // Validations 
        this.InitializeValidationRules();
    }
    
    public Ticket( long id, Flight flight, FlightClass flightClass, 
        int placeNumber, int price, Discount discount, bool isSold, float discountPrice )
    {
        Id = id;
        Flight = flight;
        FlightClass = flightClass;
        PlaceNumber = placeNumber;
        Price = price;
        Discount = discount;
        IsSold = isSold;
        DiscountPrice = discountPrice;
        
        // Validations 
        this.InitializeValidationRules();
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is Ticket o)
        {
            return Id.Equals(o.Id) && 
                   Flight.Equals(o.Flight) &&
                   FlightClass.Equals(o.FlightClass) && 
                   PlaceNumber.Equals(o.PlaceNumber) &&
                   Price.Equals(o.Price) && 
                   Discount.Equals(o.Discount) && 
                   IsSold.Equals(o.IsSold) &&
                   DiscountPrice.Equals(o.DiscountPrice);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode() 
    {
        return base.GetHashCode();
    }
}