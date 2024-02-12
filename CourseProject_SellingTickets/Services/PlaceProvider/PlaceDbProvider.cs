using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.PlaceProvider;

public class PlaceDbProvider : IPlaceDbProvider
{
    private readonly ITradeTicketsDbContextFactory _dbContextFactory;

    public PlaceDbProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<Place>> GetAllPlaces()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<PlaceDTO> placeDtos = await context.Places.
                AsNoTracking().
                Include(x => x.Photo).
                ToListAsync();

            return placeDtos.Select(aircraft => ToPlace(aircraft));
        }
    }
    
    private static Place ToPlace(PlaceDTO dto)
    {
        return new Place( dto.Id, dto.Name, dto.Description, 
            new Photo( dto.Photo.Id, dto.Photo.Name, dto.Photo.UrlPath, dto.Photo.IsDeleted ));
    } 
    
}