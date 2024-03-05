using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
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

            return placeDtos.Select(place => ToPlace(place));
        }
    }

    public async Task<IEnumerable<Place>> GetTopPlaces(int topRows = 50)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<PlaceDTO> placeDtos = await context.Places.
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                Include( x => x.Photo ).
                ToListAsync();

            return placeDtos.Select(place => ToPlace(place));
        }
    }

    public async Task<IEnumerable<Place>> GetPlacesByFilter(Expression<Func<PlaceDTO, bool>> searchFunc, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<PlaceDTO> placeDtos = await context.Places.
                Where(searchFunc).
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                Include( x => x.Photo ).
                ToListAsync();

            return placeDtos.Select(ticket => ToPlace(ticket));
        }
    }

    public async Task<IEnumerable<Place>> GetPlacesByFilterSort<TKeySelector>(Expression<Func<PlaceDTO, bool>> searchFunc, Expression<Func<PlaceDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<PlaceDTO> placeDtos = await context.Places.
                Where(searchFunc).
                OrderByModeOrDefault( sortFunc, sortMode ).
                TakeOrDefault(topRows).
                AsNoTracking().
                Include( x => x.Photo ).
                ToListAsync();

            return placeDtos.Select(place => ToPlace(place));
        }
    }

    public async Task<int> CreateOrEditPlace(Place place)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            PlaceDTO placeDto = ToPlaceDto(place);

            if (placeDto.Id.Equals(default))
                await context.Places.AddAsync(placeDto);
            else
                context.Places.Attach(placeDto).State = EntityState.Modified;

            return await context.SaveChangesAsync();
        }
    }

    public async Task<int> DeletePlace(Place place)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            PlaceDTO ticketDto = ToPlaceDto(place);
            
            context.Places.Remove(ticketDto);
            return await context.SaveChangesAsync();
        }
    }

    private static Place ToPlace(PlaceDTO dto)
    {
        return new Place( dto.Id, dto.Name, dto.Description, 
            new Photo( dto.Photo.Id, dto.Photo.Name, dto.Photo.UrlPath, dto.Photo.IsDeleted ));
    }

    private static PlaceDTO ToPlaceDto(Place place)
    {
        return new PlaceDTO
        {
            Id = place.Id,
            Name = place.Name,
            Description = place.Description,
            PhotoId = place.Photo.Id
        };
    }
    
}