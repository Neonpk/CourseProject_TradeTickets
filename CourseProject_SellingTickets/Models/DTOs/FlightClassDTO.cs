using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
[Table("flight_class")]
public class FlightClassDTO
{
    
    // Columns
    
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public System.Int64 Id { get; init; }
    
    [Column("class_name")]
    public string ClassName { get; init; }
}