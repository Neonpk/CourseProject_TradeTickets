using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

[Table("place")]
public class PlaceDTO
{
    
    // Columns
    
    [Column("id")]
    public System.Int64 Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
    
    [Column("photo_id")]
    [ForeignKey("Photo")]
    public System.Int64 PhotoId { get; set; }
    
    // Foreign keys (Navigation)
    public virtual PhotoDTO Photo { get; set; }
    
}