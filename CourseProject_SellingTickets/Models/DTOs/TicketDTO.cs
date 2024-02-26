using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
[Table("ticket")]
public class TicketDTO
{
    // Columns
    
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public System.Int64 Id { get; init; }
    
    [Column("flight_id")]
    [ForeignKey("Flight")]
    public System.Int64 FlightId { get; init; }
    
    [Column("class_id")]
    [ForeignKey("FlightClass")]
    public System.Int64 ClassId { get; init; }
    
    [Column("place_number")]
    public int PlaceNumber { get; init; }
    
    [Column("discount_id")]
    [ForeignKey("Discount")]
    public System.Int64 DiscountId { get; init; }
    
    [Column("is_sold")]
    public bool IsSold { get; init; }
    
    // Foreign keys (Navigation)
    
    public virtual FlightDTO Flight { get; init; }
    public virtual FlightClassDTO FlightClass { get; init; }
    public virtual DiscountDTO Discount { get; init; }
    
}