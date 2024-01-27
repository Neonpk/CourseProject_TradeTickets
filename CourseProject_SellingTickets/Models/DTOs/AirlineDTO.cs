using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

[Table("airline")]
public class AirlineDTO
{
    // Columns
    
    [Column("id")]
    public System.Int64 Id { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
}