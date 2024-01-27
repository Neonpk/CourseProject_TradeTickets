using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.TradeTicketsProvider;

public class DatabaseFlightDbProvider : IFlightDbProvider
{
    private readonly ITradeTicketsDbContextFactory? _dbContextFactory;
    
    public DatabaseFlightDbProvider(ITradeTicketsDbContextFactory? dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IEnumerable<Flight>> GetAllFlights()
    {
        if (_dbContextFactory!.Equals(null))
            new Exception("DbContext not existing.");
        
        using (TradeTicketsDbContext context = _dbContextFactory!.CreateDbContext())
        {
            IEnumerable<FlightDTO> flightDtos = await context.Flights.
                AsNoTracking().
                Include( x => x.Aircraft ).
                Include( x => x.Airline ). 
                Include( x => x.DeparturePlace ).
                Include( x => x.DestinationPlace ).
                Include( x => x.Aircraft!.Photo ).
                Include( x => x.DeparturePlace!.Photo ).
                Include( x => x.DestinationPlace!.Photo ).
                ToListAsync();

            return flightDtos.Select(flight => ToFlight(flight));
        }
    }
    
    private static Flight ToFlight(FlightDTO dto)
    {
        return new Flight( dto );
    }
    
}