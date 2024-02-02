using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Models;
using DynamicData;
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
            DeparturePlaceId = flight.DeparturePlace.Id,
            DepartureTime = flight.DepartureTime,
            DestinationPlaceId = flight.DestinationPlace.Id,
            ArrivalTime = flight.ArrivalTime,
            AircraftId = flight.Aircraft.Id,
            AirlineId = flight.Airline.Id,
            IsCanceled = flight.IsCanceled
        };
    }
    
}