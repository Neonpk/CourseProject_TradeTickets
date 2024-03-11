using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands;

public class LoadFlightDataCommand : ReactiveCommand<IEnumerable<Flight>, Task>
{
    private static async Task LoadDataAsync(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmDbProvider, IEnumerable<Flight> filteredFlights)
    {
        var limitRows = flightUserViewModel.LimitRows;
        
        flightUserViewModel.ErrorMessage = string.Empty;
        flightUserViewModel.IsLoading = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        try
        {
            bool hasSearching = flightUserViewModel.HasSearching;
           
            IEnumerable<Flight> flights = hasSearching ? filteredFlights! : await flightVmDbProvider.GetTopFlights(limitRows);
            
            IEnumerable<Aircraft> aircrafts = await flightVmDbProvider.GetAllAircrafts();
            IEnumerable<Airline> airlines = await flightVmDbProvider.GetAllAirlines();
            IEnumerable<Place> places = await flightVmDbProvider.GetAllPlaces();

            Dispatcher.UIThread.Post(() =>
            {

                flightUserViewModel.Aircrafts.Clear();
                flightUserViewModel.Aircrafts.AddRange(aircrafts);

                flightUserViewModel.Airlines.Clear();
                flightUserViewModel.Airlines.AddRange(airlines);

                flightUserViewModel.Places.Clear();
                flightUserViewModel.Places.AddRange(places);

                flightUserViewModel.FlightItems.Clear();
                flightUserViewModel.FlightItems.AddRange(flights);

            });

            flightUserViewModel.SortFlightsCommand!.Execute().Subscribe();
        }
        catch (Exception e)
        {
            flightUserViewModel.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
       
        flightUserViewModel.IsLoading = false;
    }

    public LoadFlightDataCommand(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider) :
        base(filteredFlights => 
                Observable.Start( async () => await LoadDataAsync(flightUserViewModel, flightVmProvider, filteredFlights)),
            canExecute: Observable.Return(true))
    {
        
    }
}