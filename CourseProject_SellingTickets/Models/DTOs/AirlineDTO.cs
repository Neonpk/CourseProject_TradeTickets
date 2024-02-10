using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

[Table("airline")]
public class AirlineDTO
{
    // Columns
    
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public System.Int64 Id { get; init; }
    
    [Column("name")]
    public required string Name { get; init; }
}