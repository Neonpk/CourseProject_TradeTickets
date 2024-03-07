using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.FlightClassProvider;

public class FlightClassVmProvider : IFlightClassVmProvider
{
    private readonly IFlightClassDbProvider _flightClassDbProvider;

    public FlightClassVmProvider(IFlightClassDbProvider? flightClassDbProvider)
    {
        _flightClassDbProvider = flightClassDbProvider!;
    }
    
    public async Task<IEnumerable<FlightClass>> GetAllFlightClasses()
    {
        return await _flightClassDbProvider.GetAllFlightClasses();
    }

    public async Task<IEnumerable<FlightClass>> GetTopFlightClasses(int topRows = 50)
    {
        return await _flightClassDbProvider.GetTopFlightClasses(topRows);
    }

    public async Task<IEnumerable<FlightClass>> GetFlightClassesByFilter(Expression<Func<FlightClassDTO, bool>> searchFunc, int topRows = -1)
    {
        return await _flightClassDbProvider.GetFlightClassesByFilter(searchFunc, topRows);
    }

    public async Task<IEnumerable<FlightClass>> GetFlightClassesByFilterSort<TKeySelector>(Expression<Func<FlightClassDTO, bool>> searchFunc, Expression<Func<FlightClassDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        return await _flightClassDbProvider.GetFlightClassesByFilterSort(searchFunc, sortFunc, sortMode, topRows);
    }

    public async Task<bool> CreateOrEditFlightClass(FlightClass flightClass)
    {
        return await _flightClassDbProvider.CreateOrEditFlightClass(flightClass);
    }

    public async Task<bool> DeleteFlightClass(FlightClass flightClass)
    {
        return await _flightClassDbProvider.DeleteFlightClass(flightClass);
    }
}