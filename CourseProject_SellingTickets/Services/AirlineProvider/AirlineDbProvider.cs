using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.AirlineProviderInterface;
using CourseProject_SellingTickets.Interfaces.DbContextsInterface;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.AirlineProvider;

public class AirlineDbProvider : IAirlineDbProvider
{
    private readonly ITradeTicketsDbContextFactory _dbContextFactory;
    
    public AirlineDbProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<Airline>> GetAllAirlines()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<AirlineDTO> airlineDtos = await context.Airlines.ToListAsync();

            return airlineDtos.Select(airline => ToAirline(airline));
        }
    }

    public async Task<IEnumerable<Airline>> GetTopAirlines(int topRows = 50)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<AirlineDTO> airlineDtos = await context.Airlines.
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return airlineDtos.Select(airline => ToAirline(airline));
        }
    }

    public async Task<IEnumerable<Airline>> GetAirlinesByFilter(Expression<Func<AirlineDTO, bool>> searchFunc, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<AirlineDTO> airlineDtos = await context.Airlines.
                Where(searchFunc).
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return airlineDtos.Select(airline => ToAirline(airline));
        }
    }

    public async Task<IEnumerable<Airline>> GetAirlinesByFilterSort<TKeySelector>(Expression<Func<AirlineDTO, bool>> searchFunc, Expression<Func<AirlineDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<AirlineDTO> airlineDtos = await context.Airlines.
                Where(searchFunc).
                OrderByModeOrDefault( sortFunc, sortMode ).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return airlineDtos.Select(airline => ToAirline(airline));
        }
    }

    public async Task<int> CreateOrEditAirline(Airline airline)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            AirlineDTO airlineDto = ToAirlineDto(airline);

            if (airlineDto.Id.Equals(default))
                context.Airlines.Add(airlineDto);
            else
                context.Airlines.Attach(airlineDto).State = EntityState.Modified;

            return await context.SaveChangesAsync();
        }
    }

    public async Task<int> DeleteAirline(Airline airline)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            AirlineDTO airlineDto = ToAirlineDto(airline);
            
            context.Airlines.Remove(airlineDto);
            return await context.SaveChangesAsync();
        }
    }

    private static Airline ToAirline(AirlineDTO dto)
    {
        return new Airline( dto.Id, dto.Name );
    }

    private static AirlineDTO ToAirlineDto(Airline airline)
    {
        return new AirlineDTO
        {
            Id = airline.Id,
            Name = airline.Name
        };
    }
}
