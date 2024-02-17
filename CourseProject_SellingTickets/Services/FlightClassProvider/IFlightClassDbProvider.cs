using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.FlightClassProvider;

public interface IFlightClassDbProvider
{
    Task<IEnumerable<FlightClass>> GetAllFlightClasses();
    Task<IEnumerable<FlightClass>> GetTopFlightClasses(int topRows = 50);

    Task<IEnumerable<FlightClass>> GetFlightClassesByFilter(Expression<Func<FlightClassDTO, bool>> searchFunc, int topRows = -1);
    
    Task<IEnumerable<FlightClass>> GetFlightClassesByFilterSort<TKeySelector>
        ( Expression<Func<FlightClassDTO, bool>> searchFunc, Expression<Func<FlightClassDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1);
    
    Task<bool> CreateOrEditFlightClass(FlightClass flightClass);
    Task<bool> DeleteFlightClass(FlightClass flightClass);
}