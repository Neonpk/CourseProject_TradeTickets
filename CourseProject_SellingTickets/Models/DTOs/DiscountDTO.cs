using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
[Table("discount")]
public class DiscountDTO
{
    // Columns
    
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public System.Int64 Id { get; init; }
    
    [Column("name")]
    public string Name { get; init; }
    
    [Column("discount_size")]
    public int DiscountSize { get; init; }
    
    [Column("description")]
    public string Description { get; init; }
    
}