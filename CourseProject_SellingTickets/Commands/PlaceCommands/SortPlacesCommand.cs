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
    private static void SortFlights(PlaceUserViewModel flightUserViewModel)
    {

        if (flightUserViewModel.IsLoading!.Value)
            return;
        
        SortMode sortMode = (SortMode)flightUserViewModel.SelectedSortMode;
        PlaceSearchSortModes searchSortModes = (PlaceSearchSortModes)flightUserViewModel.SelectedSortValue;

        Dispatcher.UIThread.Post(() =>
        {
            var placeItems = flightUserViewModel.PlaceItems;

            switch (searchSortModes)
            {
                // By Name (Country)
                case PlaceSearchSortModes.Name:
                    placeItems!.OrderByReferenceMode(x => x.Name, sortMode);
                    break;
                
                // By Description (Airport)
                case PlaceSearchSortModes.Description:
                    placeItems!.OrderByReferenceMode(x => x.Description, sortMode);
                    break;
            }
        });

    }
    
    public SortPlacesCommand(PlaceUserViewModel placeUserViewModel) :
        base(_ => Observable.Start(() => SortFlights(placeUserViewModel)), canExecute: Observable.Return(true))
    {
        
    }
}