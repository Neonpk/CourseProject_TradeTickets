using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Interfaces.AirlineProviderInterface;

public interface IAirlineVmProvider
{
    Task<IEnumerable<Airline>> GetAllAirlines();
    
    Task<IEnumerable<Airline>> GetTopAirlines(int topRows = 50);

    Task<IEnumerable<Airline>> GetAirlinesByFilter(Expression<Func<AirlineDTO, bool>> searchFunc, int topRows = -1);
    
    Task<IEnumerable<Airline>> GetAirlinesByFilterSort<TKeySelector>
        ( Expression<Func<AirlineDTO, bool>> searchFunc, Expression<Func<AirlineDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1);
    
    Task<int> CreateOrEditAirline(Airline airline);
    Task<int> DeleteAirline(Airline airline);
}
