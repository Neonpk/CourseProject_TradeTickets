using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.AircraftProvider;

public interface IAircraftVmProvider
{
    Task<IEnumerable<Aircraft>> GetAllAircrafts();
    
    Task<IEnumerable<Aircraft>> GetTopAircrafts(int topRows = 50);

    Task<IEnumerable<Aircraft>> GetAircraftsByFilter(Expression<Func<AircraftDTO, bool>> searchFunc, int topRows = -1);
    
    Task<IEnumerable<Aircraft>> GetAircraftsByFilterSort<TKeySelector>
        ( Expression<Func<AircraftDTO, bool>> searchFunc, Expression<Func<AircraftDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1);

    Task<IEnumerable<Photo>> GetAllPhotos();
    
    Task<int> CreateOrEditAircraft(Aircraft aircraft);
    Task<int> DeleteAircraft(Aircraft aircraft);
}