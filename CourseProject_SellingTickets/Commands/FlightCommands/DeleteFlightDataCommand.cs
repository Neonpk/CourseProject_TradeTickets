using System;
using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands;

public class DeleteFlightDataCommand : ReactiveCommand<Unit, Unit>
{
    private static void DeleteFlight(FlightUserViewModel flightUserViewModel, IFlightProvider? flightProvider)
    {
        flightUserViewModel.ErrorMessage = string.Empty;
        flightUserViewModel.IsLoadingEditMode = true;
        
        try
        {
            Flight? selectedFlight = flightUserViewModel.SelectedFlight;
            
            bool isDeleted = flightProvider!.DeleteFlight(selectedFlight).Result;

            flightUserViewModel.LoadFlightsCommand?.Execute();
            
            flightUserViewModel.ErrorMessage = isDeleted ? "The row was deleted." : "Failed to delete row";
        }
        catch (Exception e)
        {
            flightUserViewModel.ErrorMessage = $"Failed to delete flight data: ({e.InnerException!.InnerException!.Message})";
        }

        flightUserViewModel.IsLoadingEditMode = false;
    }

    public DeleteFlightDataCommand( FlightUserViewModel flightUserViewModel, IFlightProvider? flightProvider ) : 
        base(_ => 
            Observable.Start(()=> DeleteFlight(flightUserViewModel, flightProvider)), canExecute: Observable.Return(true))
    {
        
    }

    public override IObservable<Unit> Execute()
    {
        return base.Execute();
    }
}