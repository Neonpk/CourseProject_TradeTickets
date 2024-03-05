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
            var flightItems = flightUserViewModel.AircraftItems;

            switch (searchSortModes)
            {
                case AircraftSearchSortModes.Model:
                    flightItems!.OrderByReferenceMode(x => x.Model, sortMode);
                    break;

                case AircraftSearchSortModes.Type:
                    flightItems!.OrderByReferenceMode(x => x.Type, sortMode);
                    break;

                case AircraftSearchSortModes.TotalPlace:
                    flightItems!.OrderByReferenceMode(x => x.TotalPlace, sortMode);
                    break;
            }
        });

    }
    
    public SortAircraftsCommand(AircraftUserViewModel aircraftUserViewModel) :
        base(_ => Observable.Start(() => SortFlights(aircraftUserViewModel)), canExecute: Observable.Return(true))
    {
        
    }
}