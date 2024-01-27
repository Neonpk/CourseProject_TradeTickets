using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.AircraftProvider;

public class DatabaseAircraftDbProvider : IAircraftDbProvider
{
    private readonly ITradeTicketsDbContextFactory? _dbContextFactory;

    public DatabaseAircraftDbProvider(ITradeTicketsDbContextFactory? dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<Aircraft>> GetAllAircrafts()
    {
        if (_dbContextFactory!.Equals(null))
            new Exception("DbContext not existing.");
        
        using (TradeTicketsDbContext context = _dbContextFactory!.CreateDbContext())
        {
            IEnumerable<AircraftDTO> aircraftDtos = await context.Aircrafts.
                AsNoTracking().
                Include(x => x.Photo).
                ToListAsync();

            return aircraftDtos.Select(aircraft => ToAircraft(aircraft));
        }
    }
    
    private static Aircraft ToAircraft(AircraftDTO dto)
    {
        return new Aircraft( dto.Id, dto.Model, dto.Type, dto.TotalPlace,
            new Photo(dto!.Photo!.Name, dto.Photo.UrlPath, dto.Photo.IsDeleted) );
    }
    
}