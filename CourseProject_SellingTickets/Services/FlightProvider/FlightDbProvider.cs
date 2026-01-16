using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Interfaces.Factories;
using CourseProject_SellingTickets.Interfaces.FlightProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.FlightProvider;

public class FlightDbProvider : IFlightDbProvider
{
    private readonly ITradeTicketsDbContextFactory _dbContextFactory;
    
    public FlightDbProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<Flight>> GetAllFlights()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
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

    public async Task<IEnumerable<Flight>> GetTopFlights(int topRows = 50)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<FlightDTO> flightDtos = await context.Flights.
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
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
    
    public async Task<IEnumerable<Flight>> GetFlightsByFilter( Expression<Func<FlightDTO, bool>> searchFunc, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<FlightDTO> flightDtos = await context.Flights.
                Where(searchFunc).
                OrderByDescending( x => x.Id ).
                TakeOrDefault(topRows).
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
    
    public async Task<IEnumerable<Flight>> GetFlightsByFilterSort<TKeySelector>
        ( Expression<Func<FlightDTO, bool>> searchFunc, Expression<Func<FlightDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<FlightDTO> flightDtos = await context.Flights.
                Where(searchFunc).
                OrderByModeOrDefault( sortFunc, sortMode ).
                TakeOrDefault(topRows).
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

    public async Task<IResult<IEnumerable<Flight>>> GetFlightsByUserId(Int64 userId, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            try
            {
                IEnumerable<FlightDTO> flightDtos = await context.Flights.
                    FromSqlInterpolated($"SELECT * FROM public.get_user_flights({(int)userId})").
                    OrderBy(x => x.Id).
                    TakeOrDefault(topRows).
                    AsNoTracking(). 
                    Include( x => x.Aircraft ).
                    Include( x => x.Airline ). 
                    Include( x => x.DeparturePlace ).
                    Include( x => x.DestinationPlace ).
                    Include( x => x.Aircraft!.Photo ).
                    Include( x => x.DeparturePlace!.Photo ).
                    Include( x => x.DestinationPlace!.Photo ).
                    ToListAsync();

                return Result<IEnumerable<Flight>>.Success(flightDtos.Select(flight => ToFlight(flight)));
            }
            catch (Npgsql.PostgresException ex)
            {
                return Result<IEnumerable<Flight>>.Failure(ex.MessageText);
            }
        }
    }

    public async Task<int> CreateOrEditFlight(Flight flight)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            FlightDTO flightDto = ToFlightDto(flight);

            if (flightDto.Id.Equals(default))
                await context.Flights.AddAsync(flightDto);
            else
                context.Flights.Attach(flightDto).State = EntityState.Modified;

            return await context.SaveChangesAsync();
        }
    }

    public async Task<int> DeleteFlight(Flight flight)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            FlightDTO flightDto = ToFlightDto(flight);
            
            context.Flights.Remove(flightDto);
            return await context.SaveChangesAsync();
        }
    }
    
    private static Flight ToFlight(FlightDTO dto)
    {
        return new Flight( dto );
    }
    
    private static FlightDTO ToFlightDto(Flight flight)
    {
        return new FlightDTO
        {
            Id = flight.Id,
            FlightNumber = flight.FlightNumber,
            DeparturePlaceId = flight.DeparturePlace.Id,
            DepartureTime = flight.DepartureTime,
            DestinationPlaceId = flight.DestinationPlace.Id,
            ArrivalTime = flight.ArrivalTime,
            AircraftId = flight.Aircraft.Id,
            AirlineId = flight.Airline.Id,
            IsCanceled = flight.IsCanceled,
            Price = flight.Price
        };
    }
}
