using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.AircraftProviderInterface;
using CourseProject_SellingTickets.Interfaces.DbContextsInterface;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.AircraftProvider;

public class AircraftDbProvider : IAircraftDbProvider
{
    private readonly ITradeTicketsDbContextFactory _dbContextFactory;

    public AircraftDbProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<Aircraft>> GetAllAircrafts()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<AircraftDTO> aircraftDtos = await context.Aircrafts.
                AsNoTracking().
                Include(x => x.Photo).
                ToListAsync();

            return aircraftDtos.Select(aircraft => ToAircraft(aircraft));
        }
    }

    public async Task<IEnumerable<Aircraft>> GetTopAircrafts(int topRows = 50)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<AircraftDTO> aircraftDtos = await context.Aircrafts.
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                Include( x => x.Photo ).
                ToListAsync();

            return aircraftDtos.Select(aircraft => ToAircraft(aircraft));
        }
    }

    public async Task<IEnumerable<Aircraft>> GetAircraftsByFilter(Expression<Func<AircraftDTO, bool>> searchFunc, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<AircraftDTO> aircraftDtos = await context.Aircrafts.
                Where(searchFunc).
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                Include( x => x.Photo ).
                ToListAsync();

            return aircraftDtos.Select(ticket => ToAircraft(ticket));
        }
    }

    public async Task<IEnumerable<Aircraft>> GetAircraftsByFilterSort<TKeySelector>(Expression<Func<AircraftDTO, bool>> searchFunc, Expression<Func<AircraftDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<AircraftDTO> aircraftDtos = await context.Aircrafts.
                Where(searchFunc).
                OrderByModeOrDefault( sortFunc, sortMode ).
                TakeOrDefault(topRows).
                AsNoTracking().
                Include( x => x.Photo ).
                ToListAsync();

            return aircraftDtos.Select(aircraft => ToAircraft(aircraft));
        }
    }

    public async Task<int> CreateOrEditAircraft(Aircraft aircraft)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            AircraftDTO aircraftDto = ToAircraftDto(aircraft);

            if (aircraftDto.Id.Equals(default))
                await context.Aircrafts.AddAsync(aircraftDto);
            else
                context.Aircrafts.Attach(aircraftDto).State = EntityState.Modified;

            return await context.SaveChangesAsync();
        }
    }

    public async Task<int> DeleteAircraft(Aircraft aircraft)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            AircraftDTO ticketDto = ToAircraftDto(aircraft);
            
            context.Aircrafts.Remove(ticketDto);
            return await context.SaveChangesAsync();
        }
    }

    private static AircraftDTO ToAircraftDto(Aircraft aircraft)
    {
        return new AircraftDTO
        {
            Id = aircraft.Id,
            Model = aircraft.Model,
            Type = aircraft.Type,
            TotalPlace = aircraft.TotalPlace,
            PhotoId = aircraft.Photo.Id
        };
    }
    private static Aircraft ToAircraft(AircraftDTO dto)
    {
        return new Aircraft( dto.Id, dto.Model, dto.Type, dto.TotalPlace,
            new Photo(dto!.Photo!.Id, dto!.Photo!.Name, dto.Photo.UrlPath, dto.Photo.IsDeleted) );
    }
    
}
