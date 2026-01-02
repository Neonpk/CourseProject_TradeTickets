using System;
using System.Collections.Generic;
using System.Reactive.Linq;
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
    private static async Task LoadDataAsync(AirlineUserViewModel airlineUserViewModel, IAirlineVmProvider airlineVmProvider, IEnumerable<Airline> filteredAirlines)
    {
        var limitRows = airlineUserViewModel.LimitRows;

        airlineUserViewModel.ErrorMessage = string.Empty;
        airlineUserViewModel.IsLoading = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        try
        {
            bool hasSearching = airlineUserViewModel.HasSearching;

            IEnumerable<Airline> airlines =
                hasSearching ? filteredAirlines! : await airlineVmProvider.GetTopAirlines(limitRows);

            Dispatcher.UIThread.Post(() =>
            {
                airlineUserViewModel.AirlineItems.Clear();
                airlineUserViewModel.AirlineItems.AddRange(airlines);
            });
            
        }
        catch (Exception e)
        {
            airlineUserViewModel.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }

        airlineUserViewModel.IsLoading = false;
    }
    
    public LoadAirlineDataCommand(AirlineUserViewModel airlineUserViewModel, IAirlineVmProvider airlineVmProvider) :
        base(filteredAirlines => Observable.Start(async () => 
                await LoadDataAsync(airlineUserViewModel, airlineVmProvider, filteredAirlines) ),
            canExecute: Observable.Return(true))
    {
    }
}
