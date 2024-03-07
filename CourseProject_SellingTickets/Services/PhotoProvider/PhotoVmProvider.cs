using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.PhotoProvider;

public class PhotoVmProvider : IPhotoVmProvider
{
    private readonly IPhotoDbProvider _photoDbProvider;

    public PhotoVmProvider(IPhotoDbProvider? photoDbProvider)
    {
        _photoDbProvider = photoDbProvider!;
    }
    
    public async Task<IEnumerable<Photo>> GetAllPhotos()
    {
        return await _photoDbProvider.GetAllPhotos();
    }

    public async Task<IEnumerable<Photo>> GetTopPhotos(int topRows = 50)
    {
        return await _photoDbProvider.GetTopPhotos(topRows);
    }

    public async Task<IEnumerable<Photo>> GetPhotosByFilter(Expression<Func<PhotoDTO, bool>> searchFunc, int topRows = -1)
    {
        return await _photoDbProvider.GetPhotosByFilter(searchFunc, topRows);
    }

    public async Task<IEnumerable<Photo>> GetPhotosByFilterSort<TKeySelector>(Expression<Func<PhotoDTO, bool>> searchFunc, Expression<Func<PhotoDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        return await _photoDbProvider.GetPhotosByFilterSort(searchFunc, sortFunc, sortMode, topRows);
    }

    public async Task<int> CreateOrEditPhoto(Photo photo)
    {
        return await _photoDbProvider.CreateOrEditPhoto(photo);
    }

    public async Task<int> DeletePhoto(Photo photo)
    {
        return await _photoDbProvider.DeletePhoto(photo);
    }
}