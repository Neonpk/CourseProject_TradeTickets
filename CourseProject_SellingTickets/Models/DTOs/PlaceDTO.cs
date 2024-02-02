using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
[Table("place")]
public class PlaceDTO
{

    // Columns
    
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public System.Int64 Id { get; init; }
    
    [Column("name")]
    public string Name { get; init; }
    
    [Column("description")]
    public string Description { get; init; }
    
    [Column("photo_id")]
    [ForeignKey("Photo")]
    public System.Int64 PhotoId { get; init; }
    
    // Foreign keys (Navigation)
    public virtual PhotoDTO Photo { get; init; }
    
}