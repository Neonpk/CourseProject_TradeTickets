namespace CourseProject_SellingTickets.Models;

public class Ticket
{
    // Columns
    
    public System.Int64 Id { get; init;  }
    public Flight Flight { get; set; }
    public FlightClass FlightClass { get; set; }
    public int PlaceNumber { get; set; }
    public int Price { get; set; }
    public Discount Discount { get; set; }
    public bool IsSold { get; set; }
    public float DiscountPrice { get; }

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