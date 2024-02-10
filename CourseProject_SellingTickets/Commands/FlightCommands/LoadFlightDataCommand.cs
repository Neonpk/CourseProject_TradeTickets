using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands;

public class LoadFlightDataCommand : ReactiveCommand<IEnumerable<Flight>, Unit>
{
    
    private static void LoadDataAsync(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmDbProvider, 
        IConnectionStateProvider connectionStateProvider, IEnumerable<Flight> filteredFlights
        )
    {
        var limitRows = flightUserViewModel.LimitRows;
        
        flightUserViewModel.ErrorMessage = string.Empty;
        flightUserViewModel.IsLoading = true;

        flightUserViewModel.DatabaseHasConnected = connectionStateProvider.IsConnected().Result;
        
        try
        {
            bool hasSearching = flightUserViewModel.HasSearching;
           
            IEnumerable<Flight> flights = hasSearching ? filteredFlights! : flightVmDbProvider.GetTopFlights(limitRows).Result;
            
            IEnumerable<Aircraft> aircrafts = flightVmDbProvider.GetAllAircrafts().Result;
            IEnumerable<Airline> airlines = flightVmDbProvider.GetAllAirlines().Result;
            IEnumerable<Place> places = flightVmDbProvider.GetAllPlaces().Result;
            
            flightUserViewModel.Aircrafts.Clear();
            flightUserViewModel.Aircrafts.AddRange(aircrafts);
            
            flightUserViewModel.Airlines.Clear();
            flightUserViewModel.Airlines.AddRange(airlines);
            
            flightUserViewModel.Places.Clear();
            flightUserViewModel.Places.AddRange(places);
            
            flightUserViewModel.FlightItems.Clear();
            flightUserViewModel.FlightItems.AddRange(flights);
            
            flightUserViewModel.SortFlightsCommand!.Execute();
        }
        catch (Exception e)
        {
            flightUserViewModel.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
       
        flightUserViewModel.IsLoading = false;
    }

    public LoadFlightDataCommand(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider, IConnectionStateProvider connectionStateProvider) :
        base(filteredFlights => Observable.Start(() => LoadDataAsync(flightUserViewModel, flightVmProvider, connectionStateProvider, filteredFlights)),
            canExecute: Observable.Return(true))
    {
        
    }
}