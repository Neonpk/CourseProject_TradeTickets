namespace CourseProject_SellingTickets.Models;

public class Ticket
{
    // Columns
    
    public System.Int64 Id { get; }
    public Flight Flight { get; }
    public FlightClass FlightClass { get; }
    public int PlaceNumber { get; }
    public int Price { get; }
    public Discount Discount { get; }
    public bool IsSold { get; }
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
    
}