using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Threading;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands;

public class SortFlightsCommand : ReactiveCommand<Unit, Unit>
{
    private static void SortFlights(FlightUserViewModel flightUserViewModel)
    {

        if (flightUserViewModel.IsLoading!.Value)
            return;
        
        SortMode sortMode = (SortMode)flightUserViewModel.SelectedSortMode;
        FlightSortModes sortModes = (FlightSortModes)flightUserViewModel.SelectedSortValue;

        Dispatcher.UIThread.Post(() =>
        {
            var flightItems = flightUserViewModel.FlightItems;

            switch (sortModes)
            {
                case FlightSortModes.FlightNumber:
                    flightItems!.OrderByReferenceMode(x => x.FlightNumber, sortMode);
                    break;

                case FlightSortModes.DeparturePlace:
                    flightItems!.OrderByReferenceMode(x => x.DeparturePlace.Name, sortMode);
                    break;

                case FlightSortModes.DestinationPlace:
                    flightItems!.OrderByReferenceMode(x => x.DestinationPlace.Name, sortMode);
                    break;

                case FlightSortModes.DepartureTime:
                    flightItems!.OrderByReferenceMode(x => x.DepartureTime, sortMode);
                    break;
                case FlightSortModes.ArrivalTime:
                    flightItems!.OrderByReferenceMode(x => x.ArrivalTime, sortMode);
                    break;

                case FlightSortModes.AircraftName:
                    flightItems!.OrderByReferenceMode(x => x.Aircraft.Model!, sortMode);
                    break;

                case FlightSortModes.TotalPlace:
                    flightItems!.OrderByReferenceMode(x => x.TotalPlace, sortMode);
                    break;
                case FlightSortModes.FreePlace:
                    flightItems!.OrderByReferenceMode(x => x.FreePlace, sortMode);
                    break;

                case FlightSortModes.CanceledFlights:
                    flightItems!.OrderByReferenceMode(x => x.IsCanceled, sortMode);
                    break;

                case FlightSortModes.DurationTime:
                    flightItems!.OrderByReferenceMode(x => x.DurationTime, sortMode);
                    break;

                case FlightSortModes.InProgress:
                    flightItems!.OrderByReferenceMode(x => x.InProgress, sortMode);
                    break;

                case FlightSortModes.IsCompleted:
                    flightItems!.OrderByReferenceMode(x => x.IsCompleted, sortMode);
                    break;
            }
        });

    }

    public SortFlightsCommand(FlightUserViewModel flightUserViewModel) :
        base(_ => Observable.Start(() => SortFlights(flightUserViewModel)), canExecute: Observable.Return(true))
    {
        
    }
}