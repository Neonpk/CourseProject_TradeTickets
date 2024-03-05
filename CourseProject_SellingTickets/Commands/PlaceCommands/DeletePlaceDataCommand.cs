using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.PlaceProvider;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PlaceCommands;

public class DeletePlaceDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(PlaceUserViewModel placeUserViewModel, IPlaceVmProvider? placeVmProvider, IConnectionStateProvider connectionStateProvider)
    {
        placeUserViewModel.ErrorMessage = string.Empty;
        placeUserViewModel.IsLoadingEditMode = true;
        
        placeUserViewModel.DatabaseHasConnected = await connectionStateProvider.IsConnected();
        
        if (!placeUserViewModel.DatabaseHasConnected)
        {
            placeUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            placeUserViewModel.IsLoadingEditMode = false;
            return;
        }
        
        try
        {
            Place? selectedPlace = placeUserViewModel.SelectedPlace;
            
            var dbState = await placeVmProvider!.DeletePlace(selectedPlace);

            placeUserViewModel.SearchPlaceDataCommand?.Execute().Subscribe();
        }
        catch (Exception e)
        {
            placeUserViewModel.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }

        placeUserViewModel.IsLoadingEditMode = false;
    }

    public DeletePlaceDataCommand( PlaceUserViewModel placeUserViewModel, IPlaceVmProvider? placeVmProvider, IConnectionStateProvider connectionStateProvider ) : 
        base(_ => 
            Observable.Start(async () => await DeleteDataAsync(placeUserViewModel, placeVmProvider, connectionStateProvider)), canExecute: Observable.Return(true))
    {
        
    }
}