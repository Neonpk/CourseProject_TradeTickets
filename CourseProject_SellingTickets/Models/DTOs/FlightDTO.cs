using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

[Table("flight")]
public class FlightDTO
{
    // Columns 
    
    [Column("id")]
    public System.Int64 Id { get; set; }
    
    [Column("flight_number")]
    public System.Int64 FlightNumber { get; set; }
    
    [Column("departure_place")]
    [ForeignKey("DeparturePlace")]
    public System.Int64 DeparturePlaceId { get; set; }
    
    [Column("departure_time")]
    public System.DateTime DepartureTime { get; set; }
    
    [Column("destination_place")]
    [ForeignKey("DestinationPlace")]
    public System.Int64 DestinationPlaceId { get; set; }
    
    [Column("arrival_time")]
    public System.DateTime ArrivalTime { get; set; }
    
    [Column("aircraft_id")]
    [ForeignKey("Aircraft")]
    public System.Int64 AircraftId { get; set; }
    
    [Column("total_place")]
    public int TotalPlace { get; init; }
    
    [Column("free_place")]
    public int FreePlace { get; init; }
    
    [Column("duration_time")]
    public System.TimeSpan DurationTime { get; init; }
    
    [Column("airline_id")]
    [ForeignKey("Airline")]
    public System.Int64 AirlineId { get; set; }
    
    [Column("is_canceled")]
    public bool IsCanceled { get; set; }
    
    
    // Foreign keys (Navigation)
    
    public virtual PlaceDTO? DeparturePlace { get; set; }
    public virtual PlaceDTO? DestinationPlace { get; set; }
    public virtual AircraftDTO? Aircraft { get; set; }
    public virtual AirlineDTO? Airline { get; set; }
    
}