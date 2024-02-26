using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.VisualBasic;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands;

public class DeleteFlightDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(FlightUserViewModel flightUserViewModel, IFlightVmProvider? flightProvider, IConnectionStateProvider connectionStateProvider)
    {
        flightUserViewModel.ErrorMessage = string.Empty;
        flightUserViewModel.IsLoadingEditMode = true;
        
        flightUserViewModel.DatabaseHasConnected = await connectionStateProvider.IsConnected();
        
        if (!flightUserViewModel.DatabaseHasConnected)
        {
            flightUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            flightUserViewModel.IsLoadingEditMode = false;
            return;
        }
        
        try
        {
            Flight? selectedFlight = flightUserViewModel.SelectedFlight;
            
            var dbState = await flightProvider!.DeleteFlight(selectedFlight);

            flightUserViewModel.SearchFlightDataCommand?.Execute().Subscribe();
        }
        catch (Exception e)
        {
            flightUserViewModel.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }

        flightUserViewModel.IsLoadingEditMode = false;
    }

    public DeleteFlightDataCommand( FlightUserViewModel flightUserViewModel, IFlightVmProvider? flightProvider, IConnectionStateProvider connectionStateProvider ) : 
        base(_ => 
            Observable.Start(async () => await DeleteDataAsync(flightUserViewModel, flightProvider, connectionStateProvider)), canExecute: Observable.Return(true))
    {
        
    }
}