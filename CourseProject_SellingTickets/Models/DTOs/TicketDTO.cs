using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
[Table("ticket")]
public class TicketDTO
{
    // Columns
    
    [Column("id")]
    public System.Int64 Id { get; set; }
    
    [Column("flight_id")]
    [ForeignKey("Flight")]
    public System.Int64 FlightId { get; set; }
    
    [Column("class_id")]
    [ForeignKey("FlightClass")]
    public System.Int64 ClassId { get; set; }
    
    [Column("place_number")]
    public int PlaceNumber { get; set; }
    
    [Column("price")]
    public int Price { get; set; }
    
    [Column("discount_id")]
    [ForeignKey("Discount")]
    public System.Int64 DiscountId { get; set; }
    
    [Column("is_sold")]
    public bool IsSold { get; set; }

    [Column("discount_price")] 
    public float DiscountPrice { get; init; }
    
    // Foreign keys (Navigation)
    
    public virtual FlightDTO Flight { get; set; }
    public virtual FlightClassDTO FlightClass { get; set; }
    public virtual DiscountDTO Discount { get; set; }
    
}