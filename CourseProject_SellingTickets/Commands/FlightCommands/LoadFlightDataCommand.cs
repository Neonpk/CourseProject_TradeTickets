using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
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
    private static async Task LoadDataAsync(FlightUserViewModel flightUserVm, IFlightVmProvider flightVmDbProvider, IEnumerable<Flight> filteredFlights)
    {
        try
        {
            var limitRows = flightUserVm.LimitRows;

            flightUserVm.ErrorMessage = string.Empty;
            flightUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();

            if (!await isConnected)
            {
                flightUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }

            bool hasSearching = flightUserVm.HasSearching;
            Int64 userId = flightUserVm.UserId;

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
                    if (!result.IsSuccess) flightUserVm.ErrorMessage = result.Message;
                }
            }
            else
            {
                flights = filteredFlights;
            }

            Dispatcher.UIThread.Post(async void () =>
            {

                flightUserVm.Aircrafts.Clear();
                flightUserVm.Airlines.Clear();
                flightUserVm.Places.Clear();
                flightUserVm.FlightItems.Clear();

                flightUserVm.FlightItems.AddRange(flights);

                if (!flightUserVm.UserId.Equals(-1))
                {
                    flightUserVm.SideBarShowed = false;
                    return;
                }

                flightUserVm.Aircrafts.AddRange(await flightVmDbProvider.GetAllAircrafts());
                flightUserVm.Airlines.AddRange(await flightVmDbProvider.GetAllAirlines());
                flightUserVm.Places.AddRange(await flightVmDbProvider.GetAllPlaces());

            });

            flightUserVm.SortFlightsCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            flightUserVm.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
        finally
        {
            flightUserVm.IsLoading = false;
        }
    }

    public LoadFlightDataCommand(FlightUserViewModel flightUserVm, IFlightVmProvider flightVmProvider) :
        base(filteredFlights => 
                Observable.Start( async () => await LoadDataAsync(flightUserVm, flightVmProvider, filteredFlights)),
            canExecute: Observable.Return(true))
    {
        
    }
}
