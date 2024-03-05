using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Threading;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AircraftCommands;

public class SortAircraftsCommand : ReactiveCommand<Unit, Unit>
{
    
    private static void SortFlights(AircraftUserViewModel flightUserViewModel)
    {

        if (flightUserViewModel.IsLoading!.Value)
            return;
        
        SortMode sortMode = (SortMode)flightUserViewModel.SelectedSortMode;
        AircraftSearchSortModes searchSortModes = (AircraftSearchSortModes)flightUserViewModel.SelectedSortValue;

        Dispatcher.UIThread.Post(() =>
        {
            var aircraftItems = flightUserViewModel.AircraftItems;

            switch (searchSortModes)
            {
                // By Model
                case AircraftSearchSortModes.Model:
                    aircraftItems!.OrderByReferenceMode(x => x.Model, sortMode);
                    break;
                // By Type
                case AircraftSearchSortModes.Type:
                    aircraftItems!.OrderByReferenceMode(x => x.Type, sortMode);
                    break;
                // By TotalPlace
                case AircraftSearchSortModes.TotalPlace:
                    aircraftItems!.OrderByReferenceMode(x => x.TotalPlace, sortMode);
                    break;
            }
        });

    }
    
    public SortAircraftsCommand(AircraftUserViewModel aircraftUserViewModel) :
        base(_ => Observable.Start(() => SortFlights(aircraftUserViewModel)), canExecute: Observable.Return(true))
    {
        
    }
}