using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.FileServiceInterface;
using CourseProject_SellingTickets.Interfaces.FreeImageServiceInterface;
using CourseProject_SellingTickets.Interfaces.PhotoProviderInterface;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.PhotoProvider;

public class PhotoVmProvider : IPhotoVmProvider
{
    private readonly IPhotoDbProvider _photoDbProvider;
    private readonly IFileService _fileService;
    private readonly IFreeImageService _freeImageService;

    public PhotoVmProvider(IPhotoDbProvider photoDbProvider, IFileService fileService, IFreeImageService freeImageService)
    {
        _photoDbProvider = photoDbProvider;
        _fileService = fileService;
        _freeImageService = freeImageService;
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

    public IFileService GetFileService()
    {
        return _fileService;
    }

    public IFreeImageService GetFreeImageService()
    {
        return _freeImageService;
    }
}
