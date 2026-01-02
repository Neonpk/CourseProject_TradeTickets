using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.PlaceProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PlaceCommands;

public class SearchPlaceDataCommand : ReactiveCommand<Unit, Task<IEnumerable<Place>?>>
{
     private static async Task<IEnumerable<Place>> GetPlaceDataByFilter(IPlaceVmProvider placeVmProvider, string searchTerm, PlaceSearchSortModes searchMode, int limitRows = 50)
    {
         switch (searchMode)
         {
             // By Name (country)
             case PlaceSearchSortModes.Name:
                 return await placeVmProvider.GetPlacesByFilter(
                     x => x.Name.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);
                
             // By Description (Airport)
             case PlaceSearchSortModes.Description:
                 return await placeVmProvider.GetPlacesByFilter(
                     x => x.Description.ToLower().StartsWith(searchTerm.ToLower()),
                     limitRows);
             
             // Empty 
             default:
                 return new List<Place>();
        }
    }
    
    private static async Task<IEnumerable<Place>?> SearchDataAsync(PlaceUserViewModel placeUserViewModel, IPlaceVmProvider placeVmProvider)
    {
        int limitRows = placeUserViewModel.LimitRows;
        string searchTerm = placeUserViewModel.SearchTerm!;
        PlaceSearchSortModes selectedSearchMode = (PlaceSearchSortModes)placeUserViewModel.SelectedSearchMode;

        try
        {
            placeUserViewModel.IsLoading = true;
            IEnumerable<Place> places = await GetPlaceDataByFilter(placeVmProvider, searchTerm, selectedSearchMode, limitRows);

            return places;
        }
        catch (Exception e)
        {
            placeUserViewModel.IsLoading = false;
            placeUserViewModel.ErrorMessage = $"Не удалось найти данные: ({e.Message})";

            return null;
        }
    }
    
    public SearchPlaceDataCommand(PlaceUserViewModel placeUserViewModel, IPlaceVmProvider placeVmProvider) : 
        base(_ => Observable.Start(async () => await SearchDataAsync(placeUserViewModel, placeVmProvider)), canExecute: Observable.Return(true))
    {
        
    }
}
