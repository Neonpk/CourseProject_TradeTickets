using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

[Table("aircraft")]
public class AircraftDTO
{
    //Columns
    
    [Column("id")] 
    public System.Int64 Id { get; set; }
    
    [Column("model")]
    public string Model { get; set; }
    
    [Column("type")]
    public string Type { get; set; }
    
    [Column("total_place")]
    public int TotalPlace { get; set; }
    
    // Foreign keys (Navigation)
    
    [Column("photo_id")]
    [ForeignKey("Photo")]
    public System.Int64 PhotoId { get; set; }

    public virtual PhotoDTO Photo { get; set; }
}