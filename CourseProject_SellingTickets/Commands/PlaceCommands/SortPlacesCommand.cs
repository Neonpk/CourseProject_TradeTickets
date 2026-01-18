using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Threading;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PlaceCommands;

public class SortPlacesCommand : ReactiveCommand<Unit, Unit>
{
    private static void SortPlaces(PlaceUserViewModel placeUserVm)
    {

        if (placeUserVm.IsLoading!.Value)
            return;
        
        SortMode sortMode = (SortMode)placeUserVm.SelectedSortMode;
        PlaceSearchSortModes searchSortModes = (PlaceSearchSortModes)placeUserVm.SelectedSortValue;

        Dispatcher.UIThread.Post(() =>
        {
            var placeItems = placeUserVm.PlaceItems;

            switch (searchSortModes)
            {
                // By Name (Country)
                case PlaceSearchSortModes.Name:
                    placeItems.OrderByReferenceMode(x => x.Name, sortMode);
                    break;
                
                // By Description (Airport)
                case PlaceSearchSortModes.Description:
                    placeItems.OrderByReferenceMode(x => x.Description, sortMode);
                    break;
            }
        });

    }
    
    public SortPlacesCommand(PlaceUserViewModel placeUserVm) :
        base(_ => Observable.Start(() => SortPlaces(placeUserVm)), canExecute: Observable.Return(true))
    {
        
    }
}
