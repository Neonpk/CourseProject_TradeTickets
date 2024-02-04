using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.AirlineProvider;

public class AirlineDbProvider : IAirlineDbProvider
{
    private readonly ITradeTicketsDbContextFactory? _dbContextFactory;
    
    public AirlineDbProvider(ITradeTicketsDbContextFactory? dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<Airline>> GetAllAirlines()
    {
        if (_dbContextFactory!.Equals(null))
            new Exception("DbContext not existing.");
        
        using (TradeTicketsDbContext context = _dbContextFactory!.CreateDbContext())
        {
            IEnumerable<AirlineDTO> airlineDtos = await context.Airlines.ToListAsync();

            return airlineDtos.Select(airline => ToAirline(airline));
        }
    }
    
    private static Airline ToAirline(AirlineDTO dto)
    {
        return new Airline( dto.Id, dto.Name );
    }
    
}