using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

[Table("aircraft")]
public class AircraftDTO
{

    //Columns
    
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public System.Int64 Id { get; init; }
    
    [Column("model")]
    public required string Model { get; init; }
    
    [Column("type")]
    public required string Type { get; init; }
    
    [Column("total_place")]
    public int TotalPlace { get; init; }
    
    // Foreign keys (Navigation)
    
    [Column("photo_id")]
    [ForeignKey("Photo")]
    public System.Int64 PhotoId { get; init; }

    public virtual PhotoDTO? Photo { get; init; }
}