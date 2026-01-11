using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Interfaces.FlightProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightCommands;

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
            Int64 userId = flightUserViewModel.UserId;

            IEnumerable<Flight> flights;

            if (!hasSearching)
            {
                if (userId.Equals(-1))
                {
                    flights = await flightVmDbProvider.GetTopFlights(limitRows);
                }
                else
                {
                    var result = await flightVmDbProvider.GetFlightsByUserId(userId, limitRows);
                    flights = result.IsSuccess ? result.Value! : new List<Flight>();
                    if (!result.IsSuccess) flightUserViewModel.ErrorMessage = result.Message;
                }
            }
            else
            {
                flights = filteredFlights;
            }
            
            Dispatcher.UIThread.Post(async void() =>
            {

                flightUserViewModel.Aircrafts.Clear();
                flightUserViewModel.Airlines.Clear();
                flightUserViewModel.Places.Clear();
                flightUserViewModel.FlightItems.Clear();

                flightUserViewModel.FlightItems.AddRange(flights);

                if (!flightUserViewModel.UserId.Equals(-1))
                {
                    flightUserViewModel.SideBarShowed = false;
                    return; 
                }

                flightUserViewModel.Aircrafts.AddRange(await flightVmDbProvider.GetAllAircrafts());
                flightUserViewModel.Airlines.AddRange(await flightVmDbProvider.GetAllAirlines());
                flightUserViewModel.Places.AddRange(await flightVmDbProvider.GetAllPlaces());
                
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
