using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

[Table("discount")]
public class DiscountDTO
{
    // Columns
    
    [Column("id")]
    public System.Int64 Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("discount_size")]
    public int DiscountSize { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
    
}