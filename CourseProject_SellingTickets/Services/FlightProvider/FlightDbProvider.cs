using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CourseProject_SellingTickets.Services.TradeTicketsProvider;

public class FlightDbProvider : IFlightDbProvider
{
    private readonly ITradeTicketsDbContextFactory? _dbContextFactory;
    
    public FlightDbProvider(ITradeTicketsDbContextFactory? dbContextFactory)
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

    public async Task<IEnumerable<Flight>> GetTopFlights(int topRows = 50)
    {
        if (_dbContextFactory!.Equals(null))
            new Exception("DbContext not existing.");

        using (TradeTicketsDbContext context = _dbContextFactory!.CreateDbContext())
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
        if (_dbContextFactory!.Equals(null))
            new Exception("DbContext not existing.");

        using (TradeTicketsDbContext context = _dbContextFactory!.CreateDbContext())
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
        if (_dbContextFactory!.Equals(null))
            new Exception("DbContext not existing.");

        using (TradeTicketsDbContext context = _dbContextFactory!.CreateDbContext())
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
    
    public async Task<bool> CreateOrEditFlight(Flight? flight)
    {
        
        if (_dbContextFactory!.Equals(null))
            new Exception("DbContext not existing.");
        
        using (TradeTicketsDbContext context = _dbContextFactory!.CreateDbContext())
        {
            
            FlightDTO flightDto = ToFlightDTO(flight);
            
            if (flightDto.Id.Equals(null))
                context.Flights.Add(flightDto);
            else
                context.Flights.Attach(flightDto).State = EntityState.Modified;

            return await context.SaveChangesAsync() > 0;
        }
    }

    public async Task<bool> DeleteFlight(Flight? flight)
    {
        using (TradeTicketsDbContext context = _dbContextFactory!.CreateDbContext())
        {
            
            FlightDTO flightDto = ToFlightDTO(flight);
            
            context.Flights.Remove(flightDto);
            return await context.SaveChangesAsync() > 0;
        }
    }
    
    private static Flight ToFlight(FlightDTO dto)
    {
        return new Flight( dto );
    }
    
    private static FlightDTO ToFlightDTO(Flight? flight)
    {
        return new FlightDTO
        {
            Id = flight!.Id,
            FlightNumber = flight.FlightNumber,
            DeparturePlaceId = flight.DeparturePlace.Id!.Value,
            DepartureTime = flight.DepartureTime,
            DestinationPlaceId = flight.DestinationPlace.Id!.Value,
            ArrivalTime = flight.ArrivalTime,
            AircraftId = flight.Aircraft.Id!.Value,
            AirlineId = flight.Airline.Id!.Value,
            IsCanceled = flight.IsCanceled
        };
    }
    
}