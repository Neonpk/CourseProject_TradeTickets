using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Interfaces.PhotoProviderInterface;

public interface IPhotoDbProvider
{
    Task<IEnumerable<Photo>> GetAllPhotos();
    Task<IEnumerable<Photo>> GetTopPhotos(int topRows = 50);

    Task<IEnumerable<Photo>> GetPhotosByFilter(Expression<Func<PhotoDTO, bool>> searchFunc, int topRows = -1);
    
    Task<IEnumerable<Photo>> GetPhotosByFilterSort<TKeySelector>
        ( Expression<Func<PhotoDTO, bool>> searchFunc, Expression<Func<PhotoDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1);
    
    Task<IResult<Int64>> GenerateAvatar(string urlPath);
    
    Task<int> CreateOrEditPhoto(Photo photo);
    Task<int> DeletePhoto(Photo photo);
}
