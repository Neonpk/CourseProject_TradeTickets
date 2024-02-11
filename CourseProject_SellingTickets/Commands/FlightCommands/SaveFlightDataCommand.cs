using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using Npgsql;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using AggregateException = System.AggregateException;

namespace CourseProject_SellingTickets.Commands;

public class SaveFlightDataCommand : ReactiveCommand<Unit, Unit>
{
    private static IObservable<bool> CanExecuteCommand(FlightUserViewModel flightVm)
    {
        return flightVm.WhenAnyValue(x => x.SelectedFlight.ValidationContext.IsValid);
    }
    
    private static void SaveDataAsync( FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider )
    {
        flightUserViewModel.ErrorMessage = string.Empty;
        flightUserViewModel.IsLoadingEditMode = true;

        try
        {
            Flight selectedFlight = flightUserViewModel.SelectedFlight;
            var isSaved = flightVmProvider.CreateOrEditFlight(selectedFlight).Result;

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

    public SaveFlightDataCommand(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider) :
        base(_ => Observable.Start(() => SaveDataAsync(flightUserViewModel, flightVmProvider)), 
            canExecute: CanExecuteCommand(flightUserViewModel).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}