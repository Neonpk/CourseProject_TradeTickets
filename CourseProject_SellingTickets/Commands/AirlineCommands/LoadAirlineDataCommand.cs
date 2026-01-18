using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Interfaces.AirlineProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AirlineCommands;

public class LoadAirlineDataCommand : ReactiveCommand<IEnumerable<Airline>, Task>
{
    private static async Task LoadDataAsync(AirlineUserViewModel airlineUserVm, IAirlineVmProvider airlineVmProvider, IEnumerable<Airline> filteredAirlines)
    {
        try
        {
            var limitRows = airlineUserVm.LimitRows;

            airlineUserVm.ErrorMessage = string.Empty;
            airlineUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();

            if (!await isConnected)
            {
                airlineUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }

            bool hasSearching = airlineUserVm.HasSearching;

            IEnumerable<Airline> airlines =
                hasSearching ? filteredAirlines : await airlineVmProvider.GetTopAirlines(limitRows);

            Dispatcher.UIThread.Post(() =>
            {
                airlineUserVm.AirlineItems.Clear();
                airlineUserVm.AirlineItems.AddRange(airlines);
            });

        }
        catch (Exception e)
        {
            airlineUserVm.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
        finally
        {
            airlineUserVm.IsLoading = false;   
        }
    }
    
    public LoadAirlineDataCommand(AirlineUserViewModel airlineUserVm, IAirlineVmProvider airlineVmProvider) :
        base(filteredAirlines => Observable.Start(async () => 
                await LoadDataAsync(airlineUserVm, airlineVmProvider, filteredAirlines) ),
            canExecute: Observable.Return(true))
    {
    }
}
