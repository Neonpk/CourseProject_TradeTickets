using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Interfaces.FlightClassProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightClassCommands;

public class LoadFlightClassDataCommand : ReactiveCommand<IEnumerable<FlightClass>, Task>
{
    private static async Task LoadDataAsync(FlightClassUserViewModel flightClassUserVm, IFlightClassVmProvider flightClassVmProvider, IEnumerable<FlightClass> filteredFlightClasses)
    {
        try
        {
            var limitRows = flightClassUserVm.LimitRows;

            flightClassUserVm.ErrorMessage = string.Empty;
            flightClassUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();
            
            if (!await isConnected)
            {
                flightClassUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }
            
            bool hasSearching = flightClassUserVm.HasSearching;

            IEnumerable<FlightClass> flightClasses =
                hasSearching ? filteredFlightClasses : await flightClassVmProvider.GetTopFlightClasses(limitRows);

            Dispatcher.UIThread.Post(() =>
            {
                flightClassUserVm.FlightClassItems.Clear();
                flightClassUserVm.FlightClassItems.AddRange(flightClasses);
            });

        }
        catch (Exception e)
        {
            flightClassUserVm.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
        finally
        {
            flightClassUserVm.IsLoading = false;   
        }
    }
    
    public LoadFlightClassDataCommand(FlightClassUserViewModel flightClassUserVm, IFlightClassVmProvider flightClassVmProvider) :
        base(filteredFlightClasses => Observable.Start(async () => 
                await LoadDataAsync(flightClassUserVm, flightClassVmProvider, filteredFlightClasses) ),
            canExecute: Observable.Return(true))
    {
    }
}
