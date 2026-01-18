using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Threading;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightCommands;

public class SortFlightsCommand : ReactiveCommand<Unit, Unit>
{
    private static void SortFlights(FlightUserViewModel flightUserVm)
    {

        if (flightUserVm.IsLoading!.Value)
            return;
        
        SortMode sortMode = (SortMode)flightUserVm.SelectedSortMode;
        FlightSortModes sortModes = (FlightSortModes)flightUserVm.SelectedSortValue;

        Dispatcher.UIThread.Post(() =>
        {
            var flightItems = flightUserVm.FlightItems;

            switch (sortModes)
            {
                case FlightSortModes.FlightNumber:
                    flightItems.OrderByReferenceMode(x => x.FlightNumber, sortMode);
                    break;

                case FlightSortModes.DeparturePlace:
                    flightItems.OrderByReferenceMode(x => x.DeparturePlace.Name, sortMode);
                    break;

                case FlightSortModes.DestinationPlace:
                    flightItems.OrderByReferenceMode(x => x.DestinationPlace.Name, sortMode);
                    break;

                case FlightSortModes.DepartureTime:
                    flightItems.OrderByReferenceMode(x => x.DepartureTime, sortMode);
                    break;
                case FlightSortModes.ArrivalTime:
                    flightItems.OrderByReferenceMode(x => x.ArrivalTime, sortMode);
                    break;

                case FlightSortModes.AircraftName:
                    flightItems.OrderByReferenceMode(x => x.Aircraft.Model, sortMode);
                    break;

                case FlightSortModes.TotalPlace:
                    flightItems.OrderByReferenceMode(x => x.TotalPlace, sortMode);
                    break;
                case FlightSortModes.FreePlace:
                    flightItems.OrderByReferenceMode(x => x.FreePlace, sortMode);
                    break;

                case FlightSortModes.CanceledFlights:
                    flightItems.OrderByReferenceMode(x => x.IsCanceled, sortMode);
                    break;

                case FlightSortModes.DurationTime:
                    flightItems.OrderByReferenceMode(x => x.DurationTime, sortMode);
                    break;

                case FlightSortModes.InProgress:
                    flightItems.OrderByReferenceMode(x => x.InProgress, sortMode);
                    break;

                case FlightSortModes.IsCompleted:
                    flightItems.OrderByReferenceMode(x => x.IsCompleted, sortMode);
                    break;
                
                case FlightSortModes.Price: 
                    flightItems.OrderByReferenceMode(x => x.Price, sortMode);
                    break;
            }
        });

    }

    public SortFlightsCommand(FlightUserViewModel flightUserVm) :
        base(_ => Observable.Start(() => SortFlights(flightUserVm)), canExecute: Observable.Return(true))
    {
        
    }
}
