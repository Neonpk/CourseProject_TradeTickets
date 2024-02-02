using System;
using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands;

public class SaveFlightDataCommand : ReactiveCommand<Unit, Unit>
{
    
    private static void SaveDataAsync( FlightUserViewModel flightUserViewModel, IFlightProvider flightProvider )
    {
        flightUserViewModel.ErrorMessage = string.Empty;
        flightUserViewModel.IsLoadingEditMode = true;
        
        try
        {
            Flight? selectedFlight = flightUserViewModel.SelectedFlight;
            var isSaved = flightProvider.CreateOrEditFlight(selectedFlight).Result;
            
            flightUserViewModel.LoadFlightsCommand?.Execute();
            
            flightUserViewModel.ErrorMessage = isSaved ? "Ok" : "Failed to save";
        }
        catch (Exception e)
        {
            flightUserViewModel.ErrorMessage = $"Failed to save flight data: ({e.InnerException!.InnerException!.Message})";
        }

        flightUserViewModel.IsLoadingEditMode = false;
    }
    
    public SaveFlightDataCommand( FlightUserViewModel flightUserViewModel, IFlightProvider flightProvider ) : 
        base(_ => Observable.Start(() => 
            SaveDataAsync( flightUserViewModel, flightProvider )), canExecute: Observable.Return(true) )
    {
        
    }

    public override IObservable<Unit> Execute()
    {
        return base.Execute();
    }
}