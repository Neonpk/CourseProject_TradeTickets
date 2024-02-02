using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands;

public class LoadFlightsCommand : ReactiveCommand<Unit,Unit>
{
    
    private static void LoadData(FlightUserViewModel flightUserViewModel, IFlightProvider flightDbProvider)
    {
        flightUserViewModel.ErrorMessage = string.Empty;
        flightUserViewModel.IsLoading = true;

        try
        {
            IEnumerable<Flight> flights = flightDbProvider!.GetAllFlights().Result;
            IEnumerable<Aircraft> aircrafts = flightDbProvider!.GetAllAircrafts().Result;
            IEnumerable<Airline> airlines = flightDbProvider!.GetAllAirlines().Result;
            IEnumerable<Place> places = flightDbProvider!.GetAllPlaces().Result;
            
            flightUserViewModel.Aircrafts!.Clear();
            flightUserViewModel.Aircrafts!.AddRange(aircrafts);
            
            flightUserViewModel.Airlines!.Clear();
            flightUserViewModel.Airlines!.AddRange(airlines);
            
            flightUserViewModel.Places!.Clear();
            flightUserViewModel.Places!.AddRange(places);
            
            flightUserViewModel.FlightItems!.Clear();
            flightUserViewModel.FlightItems!.AddRange(flights);
        }
        catch (Exception e)
        {
            flightUserViewModel.ErrorMessage = $"Failed to load flight data: ({e.Message})";
        }

        flightUserViewModel.IsLoading = false;
    }

    public LoadFlightsCommand(FlightUserViewModel flightUserViewModel, IFlightProvider flightProvider) :
        base(_ => Observable.Start(() => LoadData(flightUserViewModel, flightProvider)),
            canExecute: Observable.Return(true))
    {
        
    }

    public override IObservable<Unit> Execute(Unit parameter)
    {
        return base.Execute();
    }
}