using System;
using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using DynamicData.Binding;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands;

public class SaveFlightDataCommand : ReactiveCommand<Unit, Unit>
{
    private static IObservable<bool> CanExecuteCommand(Flight selectedFlight)
    {
        return Observable.Return(true);
    }
    
    private static void SaveDataAsync( FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider )
    {
        flightUserViewModel.ErrorMessage = string.Empty;
        flightUserViewModel.IsLoadingEditMode = true;
        
        try
        {
            Flight? selectedFlight = flightUserViewModel.SelectedFlight;
            var isSaved = flightVmProvider.CreateOrEditFlight(selectedFlight).Result;

            flightUserViewModel.SearchFlightDataCommand!.Execute().Subscribe();
        }
        catch (Exception e)
        {
            flightUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.InnerException!.Message})";
        }

        flightUserViewModel.IsLoadingEditMode = false;
    }

    public SaveFlightDataCommand(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider) :
        base(_ => Observable.Start(() => SaveDataAsync(flightUserViewModel, flightVmProvider)), 
            canExecute: CanExecuteCommand(flightUserViewModel.SelectedFlight))
    {

    }
}