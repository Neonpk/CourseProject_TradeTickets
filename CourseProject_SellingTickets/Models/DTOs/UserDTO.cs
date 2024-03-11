using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
[Table("user")]
public class UserDTO
{
    [Key] 
    [Column("id")] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int64 Id { get; set; }

    [Column("mode")] 
    public string Mode { get; set; }

    [Column("password")]
    public string Password { get; set; }
}