using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.PhotoProvider;

namespace CourseProject_SellingTickets.Services.AircraftProvider;

public class AircraftVmProvider : IAircraftVmProvider
{
    private readonly IAircraftDbProvider? _aircraftDbProvider;
    private readonly IPhotoDbProvider? _photoDbProvider;

    public AircraftVmProvider(IAircraftDbProvider? aircraftVmProvider, IPhotoDbProvider? photoDbProvider)
    {
        _aircraftDbProvider = aircraftVmProvider;
        _photoDbProvider = photoDbProvider;
    }
    
    public async Task<IEnumerable<Aircraft>> GetAllAircrafts()
    {
        return await _aircraftDbProvider!.GetAllAircrafts();
    }

    public async Task<IEnumerable<Aircraft>> GetTopAircrafts(int topRows = 50)
    {
        return await _aircraftDbProvider!.GetTopAircrafts(topRows);
    }

    public async Task<IEnumerable<Aircraft>> GetAircraftsByFilter(Expression<Func<AircraftDTO, bool>> searchFunc, int topRows = -1)
    {
        return await _aircraftDbProvider!.GetAircraftsByFilter(searchFunc, topRows);
    }

    public async Task<IEnumerable<Aircraft>> GetAircraftsByFilterSort<TKeySelector>(Expression<Func<AircraftDTO, bool>> searchFunc, Expression<Func<AircraftDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        return await _aircraftDbProvider!.GetAircraftsByFilterSort(searchFunc, sortFunc, sortMode, topRows);
    }

    public async Task<IEnumerable<Photo>> GetAllPhotos()
    {
        return await _photoDbProvider!.GetAllPhotos();
    }

    public async Task<int> CreateOrEditAircraft(Aircraft aircraft)
    {
        return await _aircraftDbProvider!.CreateOrEditAircraft(aircraft);
    }

    public async Task<int> DeleteAircraft(Aircraft aircraft)
    {
        return await _aircraftDbProvider!.DeleteAircraft(aircraft);
    }
}