using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using Npgsql;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using AggregateException = System.AggregateException;

namespace CourseProject_SellingTickets.Commands;

public class SaveFlightDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(FlightUserViewModel flightVm)
    {
        return flightVm.WhenAnyValue(x => x.SelectedFlight.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync( FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider, IConnectionStateProvider connectionStateProvider )
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
            Flight selectedFlight = flightUserViewModel.SelectedFlight;
            var isSaved = await flightVmProvider.CreateOrEditFlight(selectedFlight);

            flightUserViewModel.SearchFlightDataCommand!.Execute().Subscribe();
        }
        catch (AggregateException e) when ( e.InnerException?.InnerException is NpgsqlException pgException )
        {
            flightUserViewModel.ErrorMessage = pgException.ErrorMessageFromCode(nameof(FlightUserViewModel));
        }
        catch (Exception e)
        {
            flightUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.InnerException!.Message})";
        }
        
        flightUserViewModel.IsLoadingEditMode = false;
    }

    public SaveFlightDataCommand(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider, IConnectionStateProvider connectionStateProvider) :
        base(_ => Observable.Start(() => SaveDataAsync(flightUserViewModel, flightVmProvider, connectionStateProvider)), 
            canExecute: CanExecuteCommand(flightUserViewModel).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}