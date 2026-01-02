using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.FlightClassProviderInterface;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.FlightClassProvider;

public class FlightClassDbProvider : IFlightClassDbProvider
{
    private readonly ITradeTicketsDbContextFactory _dbContextFactory;
    
    public FlightClassDbProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<FlightClass>> GetAllFlightClasses()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<FlightClassDTO> flightClassDtos = await context.FlightClasses.
                AsNoTracking().
                ToListAsync();

            return flightClassDtos.Select(flightClass => ToFlightClass(flightClass));
        }
    }

    public async Task<IEnumerable<FlightClass>> GetTopFlightClasses(int topRows = 50)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<FlightClassDTO> flightClassDtos = await context.FlightClasses.
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return flightClassDtos.Select(flightClass => ToFlightClass(flightClass));
        }
    }

    public async Task<IEnumerable<FlightClass>> GetFlightClassesByFilter(Expression<Func<FlightClassDTO, bool>> searchFunc, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<FlightClassDTO> flightClassDtos = await context.FlightClasses.
                Where(searchFunc).
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return flightClassDtos.Select(flightClass => ToFlightClass(flightClass));
        }
    }

    public async Task<IEnumerable<FlightClass>> GetFlightClassesByFilterSort<TKeySelector>(Expression<Func<FlightClassDTO, bool>> searchFunc, Expression<Func<FlightClassDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<FlightClassDTO> flightClassDtos = await context.FlightClasses.
                Where(searchFunc).
                OrderByModeOrDefault( sortFunc, sortMode ).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return flightClassDtos.Select(flightClass => ToFlightClass(flightClass));
        }
    }

    public async Task<bool> CreateOrEditFlightClass(FlightClass flightClass)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            FlightClassDTO flightClassDto = ToFlightClassDto(flightClass);

            if (flightClassDto.Id.Equals(default))
                context.FlightClasses.Add(flightClassDto);
            else
                context.FlightClasses.Attach(flightClassDto).State = EntityState.Modified;

            return await context.SaveChangesAsync() > 0;
        }
    }

    public async Task<bool> DeleteFlightClass(FlightClass flightClass)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            FlightClassDTO flightClassDto = ToFlightClassDto(flightClass);
            
            context.FlightClasses.Remove(flightClassDto);
            return await context.SaveChangesAsync() > 0;
        }
    }
    
    private static FlightClass ToFlightClass(FlightClassDTO flightClassDto)
    {
        return new FlightClass( flightClassDto.Id, flightClassDto.ClassName );
    }
    
    private static FlightClassDTO ToFlightClassDto(FlightClass flightClass)
    {
        return new FlightClassDTO
        {
            Id = flightClass.Id,
            ClassName = flightClass.ClassName
        };
    }
}
