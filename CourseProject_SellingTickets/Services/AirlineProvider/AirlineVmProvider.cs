using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.AirlineProvider;

public class AirlineVmProvider : IAirlineVmProvider
{

    private readonly IAirlineDbProvider _airlineDbProvider;
    
    public AirlineVmProvider(IAirlineDbProvider? airlineDbProvider)
    {
        _airlineDbProvider = airlineDbProvider!;
    }
    
    public async Task<IEnumerable<Airline>> GetAllAirlines()
    {
        return await _airlineDbProvider.GetAllAirlines();
    }

    public async Task<IEnumerable<Airline>> GetTopAirlines(int topRows = 50)
    {
        return await _airlineDbProvider.GetTopAirlines(topRows);
    }

    public async Task<IEnumerable<Airline>> GetAirlinesByFilter(Expression<Func<AirlineDTO, bool>> searchFunc, int topRows = -1)
    {
        return await _airlineDbProvider.GetAirlinesByFilter(searchFunc, topRows);
    }

    public async Task<IEnumerable<Airline>> GetAirlinesByFilterSort<TKeySelector>(Expression<Func<AirlineDTO, bool>> searchFunc, Expression<Func<AirlineDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        return await _airlineDbProvider.GetAirlinesByFilterSort(searchFunc, sortFunc, sortMode, topRows);
    }

    public async Task<int> CreateOrEditAirline(Airline airline)
    {
        return await _airlineDbProvider.CreateOrEditAirline(airline);
    }

    public async Task<int> DeleteAirline(Airline airline)
    {
        return await _airlineDbProvider.DeleteAirline(airline);
    }
}