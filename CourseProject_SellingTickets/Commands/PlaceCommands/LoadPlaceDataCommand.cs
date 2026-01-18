using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Interfaces.PlaceProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PlaceCommands;

public class LoadPlaceDataCommand : ReactiveCommand<IEnumerable<Place>, Task>
{
    private static async Task LoadDataAsync(PlaceUserViewModel placeUserVm, IPlaceVmProvider placeVmProvider, IEnumerable<Place> filteredPlaces)
    {
        try
        {
            var limitRows = placeUserVm.LimitRows;
        
            placeUserVm.ErrorMessage = string.Empty;
            placeUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();

            if (!await isConnected)
            {
                placeUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }
            
            bool hasSearching = placeUserVm.HasSearching;

            IEnumerable<Place> places = hasSearching ? filteredPlaces : await placeVmProvider.GetTopPlaces(limitRows);

            IEnumerable<Photo> photos = await placeVmProvider.GetAllPhotos();

            Dispatcher.UIThread.Post(() =>
            {
                placeUserVm.Photos.Clear();
                placeUserVm.Photos.AddRange(photos);

                placeUserVm.PlaceItems.Clear();
                placeUserVm.PlaceItems.AddRange(places);

            });

            placeUserVm.SortAircraftCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            placeUserVm.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
        finally
        {
            placeUserVm.IsLoading = false;   
        }
    }
    
    public LoadPlaceDataCommand(PlaceUserViewModel placeUserVm, IPlaceVmProvider placeVmProvider) :
        base(filteredPlaces => 
                Observable.Start( async () => await LoadDataAsync(placeUserVm, placeVmProvider, filteredPlaces)),
            canExecute: Observable.Return(true))
    {
        
    }
}
