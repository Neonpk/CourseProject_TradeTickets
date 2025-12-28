using System;
using System.Collections.Generic;
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

    [Column("login")] 
    public required string Login { get; set; }

    [Column("name")] 
    public required string Name { get; set; }

    [Column("role")] 
    public required string Role { get; set; }

    [Column("password")] 
    public required string Password { get; set; }

    [Column("balance")] 
    public required decimal Balance { get; set; }

    public virtual ICollection<TicketDTO> Tickets { get; private set; }
}
