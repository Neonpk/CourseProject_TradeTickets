using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.FlightClassProvider;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightClassCommands;

public class LoadFlightClassDataCommand : ReactiveCommand<IEnumerable<FlightClass>, Task>
{
    private static async Task LoadDataAsync(FlightClassUserViewModel flightClassUserViewModel, IFlightClassVmProvider flightClassVmProvider, IEnumerable<FlightClass> filteredFlightClasses)
    {
        var limitRows = flightClassUserViewModel.LimitRows;

        flightClassUserViewModel.ErrorMessage = string.Empty;
        flightClassUserViewModel.IsLoading = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        try
        {
            bool hasSearching = flightClassUserViewModel.HasSearching;

            IEnumerable<FlightClass> flightClasses =
                hasSearching ? filteredFlightClasses! : await flightClassVmProvider.GetTopFlightClasses(limitRows);

            Dispatcher.UIThread.Post(() =>
            {
                flightClassUserViewModel.FlightClassItems.Clear();
                flightClassUserViewModel.FlightClassItems.AddRange(flightClasses);
            });
            
        }
        catch (Exception e)
        {
            flightClassUserViewModel.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }

        flightClassUserViewModel.IsLoading = false;
    }
    
    public LoadFlightClassDataCommand(FlightClassUserViewModel flightClassUserViewModel, IFlightClassVmProvider flightClassVmProvider) :
        base(filteredFlightClasses => Observable.Start(async () => 
                await LoadDataAsync(flightClassUserViewModel, flightClassVmProvider, filteredFlightClasses) ),
            canExecute: Observable.Return(true))
    {
    }
}