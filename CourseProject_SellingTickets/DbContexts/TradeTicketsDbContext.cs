using System.ComponentModel.DataAnnotations.Schema;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.DbContexts;

public class TradeTicketsDbContext : DbContext
{
    public TradeTicketsDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<FlightDTO> Flights { get; set; }
    public DbSet<AircraftDTO> Aircrafts { get; set; }
    public DbSet<AirlineDTO> Airlines { get; set; }
    public DbSet<DiscountDTO> Discounts { get; set; }
    public DbSet<FlightClassDTO> FlightClasses { get; set; }
    public DbSet<PhotoDTO> Photos { get; set; }
    public DbSet<PlaceDTO> Places { get; set; }
    public DbSet<TicketDTO> Tickets { get; set; }
    
    
    
}