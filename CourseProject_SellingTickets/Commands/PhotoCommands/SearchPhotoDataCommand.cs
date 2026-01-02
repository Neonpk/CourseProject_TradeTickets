using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.PhotoProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PhotoCommands;

public class SearchPhotoDataCommand : ReactiveCommand<Unit, Task<IEnumerable<Photo>?>>
{
    private static async Task<IEnumerable<Photo>> GetFlightClassDataByFilter(IPhotoVmProvider photoVmProvider, string searchTerm, PhotoSearchSortModes searchMode, int limitRows = 50)
    {
         switch (searchMode)
         {
             // By Name 
             case PhotoSearchSortModes.Name:
                 return await photoVmProvider.GetPhotosByFilter(
                     x => x.Name.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);
                
             // By UrlPath
             case PhotoSearchSortModes.UrlPath:
                 return await photoVmProvider.GetPhotosByFilter(
                     x => x.UrlPath.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);
             // Empty 
             default:
                 return new List<Photo>();
        }
    }
    private static async Task<IEnumerable<Photo>?> SearchDataAsync(PhotoUserViewModel photoUserViewModel, IPhotoVmProvider photoVmProvider)
    {
        int limitRows = photoUserViewModel.LimitRows;
        string searchTerm = photoUserViewModel.SearchTerm!;
        PhotoSearchSortModes selectedSearchMode = (PhotoSearchSortModes)photoUserViewModel.SelectedSearchMode;
        
        try
        {
            photoUserViewModel.IsLoading = true;
            IEnumerable<Photo> photos = await GetFlightClassDataByFilter(photoVmProvider, searchTerm, selectedSearchMode, limitRows);

            return photos;
        }
        catch (Exception e)
        {
            photoUserViewModel.IsLoading = false;
            photoUserViewModel.ErrorMessage = $"Не удалось найти данные: ({e.Message})";

            return null;
        }
    }
    
    public SearchPhotoDataCommand(PhotoUserViewModel photoUserViewModel, IPhotoVmProvider photoVmProvider) : 
        base(_ => Observable.Start(async () => await SearchDataAsync(photoUserViewModel, photoVmProvider)), canExecute: Observable.Return(true))
    {
    }
}
