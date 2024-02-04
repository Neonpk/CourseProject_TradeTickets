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
    
    private static void SaveDataAsync( FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider )
    {
        flightUserViewModel.ErrorMessage = string.Empty;
        flightUserViewModel.IsLoadingEditMode = true;
        
        try
        {
            Flight? selectedFlight = flightUserViewModel.SelectedFlight;
            var isSaved = flightVmProvider.CreateOrEditFlight(selectedFlight).Result;
            
            flightUserViewModel.LoadFlightsCommand?.Execute();
        }
        catch (Exception e)
        {
            flightUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.InnerException!.Message})";
        }

        flightUserViewModel.IsLoadingEditMode = false;
    }

    public SaveFlightDataCommand(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider) :
        base(_ => Observable.Start(() =>
            SaveDataAsync(flightUserViewModel, flightVmProvider)), canExecute: Observable.Return(true))
    {

    }
}