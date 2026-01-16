using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Interfaces.FlightProviderInterface;

public interface IFlightVmProvider
{
    Task<IEnumerable<Flight>> GetAllFlights();
    Task<IEnumerable<Flight>> GetTopFlights(int topRows = 50);
    Task<IEnumerable<Flight>> GetFlightsByFilter(Expression<Func<FlightDTO, bool>> searchFunc, int topRows = -1);
    Task<IEnumerable<Flight>> GetFlightsByFilterSort<TKeySelector>
        (Expression<Func<FlightDTO, bool>> searchFunc, Expression<Func<FlightDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1);
    
    Task<IResult<IEnumerable<Flight>>> GetFlightsByUserId(Int64 userId, int topRows = -1);
    
    Task<IEnumerable<Airline>> GetAllAirlines();
    Task<IEnumerable<Aircraft>> GetAllAircrafts();
    Task<IEnumerable<Place>> GetAllPlaces();
    
    Task<int> CreateOrEditFlight(Flight flight);
    Task<int> DeleteFlight(Flight flight);
}
