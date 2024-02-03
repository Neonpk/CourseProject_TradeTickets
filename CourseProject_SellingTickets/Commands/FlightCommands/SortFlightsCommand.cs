using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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
        FlightValue value = (FlightValue)flightUserViewModel.SelectedSortValue;
        
        var flightItems = flightUserViewModel.FlightItems;
        
        switch (value)
        {
            case FlightValue.FlightNumber:
                flightItems!.OrderByReferenceMode(x => x.FlightNumber, sortMode);
                break;
            
            case FlightValue.DeparturePlace:
                flightItems!.OrderByReferenceMode(x => x.DeparturePlace.Name, sortMode);
                break;
            case FlightValue.DestinationPlace:
                flightItems!.OrderByReferenceMode(x => x.DestinationPlace.Name, sortMode);
                break;
            
            case FlightValue.DepartureTime:
                flightItems!.OrderByReferenceMode(x => x.DepartureTime, sortMode);
                break;
            case FlightValue.ArrivalTime:
                flightItems!.OrderByReferenceMode(x => x.ArrivalTime, sortMode);
                break;

            case FlightValue.AircraftName:
                flightItems!.OrderByReferenceMode(x => x.Aircraft.Model!, sortMode);
                break;

            case FlightValue.TotalPlace:
                flightItems!.OrderByReferenceMode(x => x.TotalPlace, sortMode);
                break;
            case FlightValue.FreePlace:
                flightItems!.OrderByReferenceMode(x => x.FreePlace, sortMode);
                break;

            case FlightValue.CanceledFlights:
                flightItems!.OrderByReferenceMode(x => x.IsCanceled, sortMode);
                break;
            
            case FlightValue.DurationTime:
                flightItems!.OrderByReferenceMode(x => x.DurationTime, sortMode);
                break;
            
            case FlightValue.InProgress:
                flightItems!.OrderByReferenceMode(x => x.InProgress, sortMode);
                break;
            
            case FlightValue.IsCompleted:
                flightItems!.OrderByReferenceMode(x => x.IsCompleted, sortMode);
                break;
        }
        
    }

    public SortFlightsCommand(FlightUserViewModel flightUserViewModel) :
        base(_ => Observable.Start(() => SortFlights(flightUserViewModel)), canExecute: Observable.Return(true))
    {
        
    }

    public override IObservable<Unit> Execute()
    {
        return base.Execute();
    }
}