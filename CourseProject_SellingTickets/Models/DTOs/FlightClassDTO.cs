using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
[Table("flight_class")]
public class FlightClassDTO
{
    
    // Columns
    
    [Column("id")]
    public System.Int64 Id { get; set; }
    
    [Column("class_name")]
    public string ClassName { get; set; }
}