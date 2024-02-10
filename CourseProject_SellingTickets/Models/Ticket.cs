using CourseProject_SellingTickets.ViewModels;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Ticket : ObservableObject
{
    // Columns

    private System.Int64? _id;
    public System.Int64? Id { get => _id; set { _id = value; OnPropertyChanged(nameof(Id)); } }

    private Flight _flight;
    public Flight Flight { get => _flight; set { _flight = value; OnPropertyChanged(nameof(Flight)); } }

    private FlightClass _flightClass;
    public FlightClass FlightClass { get => _flightClass; set { _flightClass = value; OnPropertyChanged(nameof(FlightClass)); } }

    private int _placeNumber;
    public int PlaceNumber { get => _placeNumber; set { _placeNumber = value; OnPropertyChanged(nameof(PlaceNumber)); } }

    private int _price;
    public int Price { get => _price; set { _price = value;  OnPropertyChanged(nameof(Price)); } }

    private Discount _discount;
    public Discount Discount { get => _discount; set { _discount = value; OnPropertyChanged(nameof(Discount)); } }

    private bool _isSold;
    public bool IsSold { get => _isSold; set { _isSold = value; OnPropertyChanged(nameof(IsSold)); } }

    // Non Observable
    public float DiscountPrice { get; }

    public Ticket()
    {
        Id = null;
    }
    
    public Ticket( System.Int64 id, Flight flight, FlightClass flightClass, 
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