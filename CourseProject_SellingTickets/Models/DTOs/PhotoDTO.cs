using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

[Table("photo")]
public class PhotoDTO
{
    // Columns 
    
    [Column("id")]
    public System.Int64 Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("url_path")]
    public string UrlPath { get; set; }
    
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
    
}