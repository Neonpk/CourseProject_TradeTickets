using System;
using CourseProject_SellingTickets.DbContexts.ModelConfigurations;
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
    public DbSet<UserDTO> Users { get; set; }
    
    // Postgres DateTime (timestamp) Format 
    [DbFunction("to_char", "")]
    public static string DateTimeFormatToString(DateTime value, string format) => value.ToString(format);
    
    // Postgres StringFormat 
    [DbFunction("FORMAT", "", IsBuiltIn = true)]
    public static string FormatSqlParams(string formatString, params string[] args) => 
        throw new NotSupportedException("Only for LINQ translation.");

    // Generate custom model builders 
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradeTicketsDbContext).Assembly);
}
