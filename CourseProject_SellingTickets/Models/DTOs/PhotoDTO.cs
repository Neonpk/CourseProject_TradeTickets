using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
[Table("photo")]
public class PhotoDTO
{
    // Columns 
    
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public System.Int64 Id { get; init; }
    
    [Column("name")]
    public string Name { get; init; }
    
    [Column("url_path")]
    public string UrlPath { get; init; }
    
    [Column("is_deleted")]
    public bool IsDeleted { get; init; }
    
}