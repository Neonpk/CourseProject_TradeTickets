using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.TradeTicketsProvider;

public interface IFlightDbProvider
{
    Task<IEnumerable<Flight>> GetAllFlights();
    Task<IEnumerable<Flight>> GetTopFlights(int topRows = 50);

    Task<IEnumerable<Flight>> GetFlightsByFilter(Expression<Func<FlightDTO, bool>> searchFunc, int topRows = -1);
    
    Task<IEnumerable<Flight>> GetFlightsByFilterSort<TKeySelector>
        ( Expression<Func<FlightDTO, bool>> searchFunc, Expression<Func<FlightDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1);
    
    Task<bool> CreateOrEditFlight(Flight? flight);
    Task<bool> DeleteFlight(Flight? flight);
}