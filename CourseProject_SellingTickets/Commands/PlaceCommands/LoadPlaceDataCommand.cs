using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.PlaceProvider;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PlaceCommands;

public class LoadPlaceDataCommand : ReactiveCommand<IEnumerable<Place>, Task>
{
    private static async Task LoadDataAsync(PlaceUserViewModel placeUserViewModel, IPlaceVmProvider placeVmProvider, IEnumerable<Place> filteredPlaces
    )
    {
        var limitRows = placeUserViewModel.LimitRows;
        
        placeUserViewModel.ErrorMessage = string.Empty;
        placeUserViewModel.IsLoading = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        try
        {
            bool hasSearching = placeUserViewModel.HasSearching;
           
            IEnumerable<Place> places = hasSearching ? filteredPlaces! : await placeVmProvider.GetTopPlaces(limitRows);
            
            IEnumerable<Photo> photos = await placeVmProvider.GetAllPhotos();

            Dispatcher.UIThread.Post(() =>
            {
                placeUserViewModel.Photos.Clear();
                placeUserViewModel.Photos.AddRange(photos);

                placeUserViewModel.PlaceItems.Clear();
                placeUserViewModel.PlaceItems.AddRange(places);

            });

            placeUserViewModel.SortAircraftCommand!.Execute().Subscribe();
        }
        catch (Exception e)
        {
            placeUserViewModel.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
       
        placeUserViewModel.IsLoading = false;
    }
    
    public LoadPlaceDataCommand(PlaceUserViewModel placeUserViewModel, IPlaceVmProvider placeVmProvider) :
        base(filteredPlaces => 
                Observable.Start( async () => await LoadDataAsync(placeUserViewModel, placeVmProvider, filteredPlaces)),
            canExecute: Observable.Return(true))
    {
        
    }
}