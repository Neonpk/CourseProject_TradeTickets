using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.FlightProvider;

public interface IFlightVmProvider
{
    public Task<IEnumerable<Flight>> GetAllFlights();
    public Task<IEnumerable<Flight>> GetTopFlights(int topRows = 50);
    public Task<IEnumerable<Flight>> GetFlightsByFilter(Expression<Func<FlightDTO, bool>> searchFunc, int topRows = -1);
    public Task<IEnumerable<Flight>> GetFlightsByFilterSort<TKeySelector>
        (Expression<Func<FlightDTO, bool>> searchFunc, Expression<Func<FlightDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1);
    
    public Task<IEnumerable<Airline>> GetAllAirlines();
    public Task<IEnumerable<Aircraft>> GetAllAircrafts();
    public Task<IEnumerable<Place>> GetAllPlaces();

    public Task<bool> CreateOrEditFlight(Flight flight);
    public Task<bool> DeleteFlight(Flight flight);
}