using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.TradeTicketsProvider;

public class DatabaseFlightProvider : IFlightProvider
{
    private readonly ITradeTicketsDbContextFactory _dbContextFactory;
    
    public DatabaseFlightProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<Flight>> GetAllFlights()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<FlightDTO> flightDtos = await context.Flights.ToListAsync();
            
            return flightDtos.Select(flight => ToFlight(flight));
        }
    }
    
    private static Flight ToFlight(FlightDTO dto)
    {
        return new Flight( dto );
    }
    
}