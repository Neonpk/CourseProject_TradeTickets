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
    public required Int64 Id { get; init; }

    [Column("login")] 
    public required string Login { get; init; }

    [Column("name")] 
    public required string Name { get; init; }

    [Column("role")] 
    public required string Role { get; init; }

    [Column("password")] 
    public required string Password { get; init; }

    [Column("balance")] 
    public required decimal Balance { get; init; }

    [Column("discount_id")]
    [ForeignKey("Discount")]
    public required Int64 DiscountId { get; init; }
    
    // Foreign keys (Navigation)
    public virtual DiscountDTO Discount { get; init; }
    
    public virtual ICollection<TicketDTO> Tickets { get; private set; }
}
