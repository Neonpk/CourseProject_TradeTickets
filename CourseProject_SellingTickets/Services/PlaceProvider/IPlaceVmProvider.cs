using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.PlaceProvider;

public interface IPlaceVmProvider
{
    Task<IEnumerable<Place>> GetAllPlaces();
    
    Task<IEnumerable<Place>> GetTopPlaces(int topRows = 50);

    Task<IEnumerable<Place>> GetPlacesByFilter(Expression<Func<PlaceDTO, bool>> searchFunc, int topRows = -1);
    
    Task<IEnumerable<Place>> GetPlacesByFilterSort<TKeySelector>
        ( Expression<Func<PlaceDTO, bool>> searchFunc, Expression<Func<PlaceDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1);

    Task<IEnumerable<Photo>> GetAllPhotos();
    
    Task<int> CreateOrEditPlace(Place place);
    Task<int> DeletePlace(Place place);
}