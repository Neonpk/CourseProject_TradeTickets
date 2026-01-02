using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.PhotoProviderInterface;
using CourseProject_SellingTickets.Interfaces.PlaceProviderInterface;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.PlaceProvider;

public class PlaceVmProvider : IPlaceVmProvider
{
    private readonly IPlaceDbProvider? _placeDbProvider;
    private readonly IPhotoDbProvider? _photoDbProvider;

    public PlaceVmProvider(IPlaceDbProvider? placeDbProvider, IPhotoDbProvider? photoDbProvider)
    {
        _placeDbProvider = placeDbProvider;
        _photoDbProvider = photoDbProvider;
    }
    
    public async Task<IEnumerable<Place>> GetAllPlaces()
    {
        return await _placeDbProvider!.GetAllPlaces();
    }

    public async Task<IEnumerable<Place>> GetTopPlaces(int topRows = 50)
    {
        return await _placeDbProvider!.GetTopPlaces(topRows);
    }

    public async Task<IEnumerable<Place>> GetPlacesByFilter(Expression<Func<PlaceDTO, bool>> searchFunc, int topRows = -1)
    {
        return await _placeDbProvider!.GetPlacesByFilter(searchFunc, topRows);
    }

    public async Task<IEnumerable<Place>> GetPlacesByFilterSort<TKeySelector>(Expression<Func<PlaceDTO, bool>> searchFunc, Expression<Func<PlaceDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        return await _placeDbProvider!.GetPlacesByFilterSort(searchFunc, sortFunc, sortMode, topRows);
    }

    public async Task<IEnumerable<Photo>> GetAllPhotos()
    {
        return await _photoDbProvider!.GetAllPhotos();
    }

    public async Task<int> CreateOrEditPlace(Place place)
    {
        return await _placeDbProvider!.CreateOrEditPlace(place);
    }

    public async Task<int> DeletePlace(Place place)
    {
        return await _placeDbProvider!.DeletePlace(place);
    }
}
