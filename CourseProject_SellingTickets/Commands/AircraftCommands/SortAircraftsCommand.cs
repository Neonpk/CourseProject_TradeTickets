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
    private static void SortAircrafts(AircraftUserViewModel aircraftUserVm)
    {
        if (aircraftUserVm.IsLoading!.Value)
            return;
        
        SortMode sortMode = (SortMode)aircraftUserVm.SelectedSortMode;
        AircraftSearchSortModes searchSortModes = (AircraftSearchSortModes)aircraftUserVm.SelectedSortValue;

        Dispatcher.UIThread.Post(() =>
        {
            var aircraftItems = aircraftUserVm.AircraftItems;

            switch (searchSortModes)
            {
                // By Model
                case AircraftSearchSortModes.Model:
                    aircraftItems.OrderByReferenceMode(x => x.Model, sortMode);
                    break;
                // By Type
                case AircraftSearchSortModes.Type:
                    aircraftItems.OrderByReferenceMode(x => x.Type, sortMode);
                    break;
                // By TotalPlace
                case AircraftSearchSortModes.TotalPlace:
                    aircraftItems.OrderByReferenceMode(x => x.TotalPlace, sortMode);
                    break;
            }
        });
    }
    
    public SortAircraftsCommand(AircraftUserViewModel aircraftUserVm) :
        base(_ => Observable.Start(() => SortAircrafts(aircraftUserVm)), canExecute: Observable.Return(true))
    {
        
    }
}
