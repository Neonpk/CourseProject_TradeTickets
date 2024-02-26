using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Models;

[Table("flight")]
public class FlightDTO
{

    // Columns 

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public System.Int64 Id { get; init; }
    
    [Column("flight_number")]
    public System.Int64 FlightNumber { get; init; }
    
    [Column("departure_place")]
    [ForeignKey("DeparturePlace")]
    public System.Int64 DeparturePlaceId { get; init; }
    
    [Column("departure_time")]
    public System.DateTime DepartureTime { get; init; }
    
    [Column("destination_place")]
    [ForeignKey("DestinationPlace")]
    public System.Int64 DestinationPlaceId { get; init; }
        
    [Column("arrival_time")]
    public System.DateTime ArrivalTime { get; init; }
    
    [Column("aircraft_id")]
    [ForeignKey("Aircraft")]
    public System.Int64 AircraftId { get; init; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("total_place")]
    public int TotalPlace { get; init; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("free_place")]
    public int FreePlace { get; init; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("duration_time")]
    public System.TimeSpan DurationTime { get; init; }
    
    [Column("airline_id")]
    [ForeignKey("Airline")]
    public System.Int64 AirlineId { get; init; }
    
    [Column("is_canceled")]
    public bool IsCanceled { get; init; }
    
    [Column("price")]
    public int Price { get; init; }
    
    // Foreign keys (Navigation)
    
    public virtual PlaceDTO? DeparturePlace { get; init; }
    public virtual PlaceDTO? DestinationPlace { get; init; }
    public virtual AircraftDTO? Aircraft { get; init; }
    public virtual AirlineDTO? Airline { get; init; }
    
}