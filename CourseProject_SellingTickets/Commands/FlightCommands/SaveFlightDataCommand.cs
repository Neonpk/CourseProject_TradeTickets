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
using AggregateException = System.AggregateException;

namespace CourseProject_SellingTickets.Commands;

public class SaveFlightDataCommand : ReactiveCommand<Unit, Unit>
{
    private static bool ValidationRules(
        DateTime departureTime, DateTime arrivalTime, 
        long? departurePlaceId, long? destinationPlaceId, 
        long? aircraftId, long? airlineId
        )
    {
        if (!aircraftId.HasValue || !airlineId.HasValue)
            return false;
        
        if (!departurePlaceId.HasValue || !destinationPlaceId.HasValue)
            return false;
        
        if (departureTime.CompareTo(arrivalTime) >= 0)
            return false;

        if (departurePlaceId.Value.CompareTo(destinationPlaceId.Value) == 0)
            return false;

        return true;
    }
    
    private static IObservable<bool> CanExecuteCommand(FlightUserViewModel flightVm)
    {
        return flightVm.WhenAnyValue(
            x => x.SelectedFlight.DepartureTime,
            x => x.SelectedFlight.ArrivalTime,
            x => x.SelectedFlight.DeparturePlace.Id,
            x => x.SelectedFlight.DestinationPlace.Id,
            x => x.SelectedFlight.Aircraft.Id,
            x => x.SelectedFlight.Airline.Id,
            ValidationRules).DistinctUntilChanged();
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
            flightUserViewModel.ErrorMessage = pgException.ErrorMessageFromCode(typeof(FlightUserViewModel));
        }
        catch (Exception e)
        {
            flightUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.InnerException!.Message})";
        }
        
        flightUserViewModel.IsLoadingEditMode = false;
    }

    public SaveFlightDataCommand(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider) :
        base(_ => Observable.Start(() => SaveDataAsync(flightUserViewModel, flightVmProvider)), 
            canExecute: CanExecuteCommand(flightUserViewModel).ObserveOn(AvaloniaScheduler.Instance))
    {

    }
}