using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AircraftCommands;

public class LoadAircraftDataCommand : ReactiveCommand<IEnumerable<Aircraft>, Task>
{
    private static async Task LoadDataAsync(AircraftUserViewModel aircraftUserViewModel, IAircraftVmProvider aircraftVmProvider, 
        IConnectionStateProvider connectionStateProvider, IEnumerable<Aircraft> filteredAircrafts
    )
    {
        var limitRows = aircraftUserViewModel.LimitRows;
        
        aircraftUserViewModel.ErrorMessage = string.Empty;
        aircraftUserViewModel.IsLoading = true;

        aircraftUserViewModel.DatabaseHasConnected = await connectionStateProvider.IsConnected();
        
        try
        {
            bool hasSearching = aircraftUserViewModel.HasSearching;
           
            IEnumerable<Aircraft> aircrafts = hasSearching ? filteredAircrafts! : await aircraftVmProvider.GetTopAircrafts(limitRows);
            
            IEnumerable<Photo> photos = await aircraftVmProvider.GetAllPhotos();

            Dispatcher.UIThread.Post(() =>
            {
                aircraftUserViewModel.Photos.Clear();
                aircraftUserViewModel.Photos.AddRange(photos);

                aircraftUserViewModel.AircraftItems.Clear();
                aircraftUserViewModel.AircraftItems.AddRange(aircrafts);

            });

            aircraftUserViewModel.SortAircraftCommand!.Execute().Subscribe();
        }
        catch (Exception e)
        {
            aircraftUserViewModel.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
       
        aircraftUserViewModel.IsLoading = false;
    }
    
    public LoadAircraftDataCommand(AircraftUserViewModel aircraftUserViewModel, IAircraftVmProvider aircraftVmProvider, IConnectionStateProvider connectionStateProvider) :
        base(filteredFlights => 
                Observable.Start( async () => await LoadDataAsync(aircraftUserViewModel, aircraftVmProvider, connectionStateProvider, filteredFlights)),
            canExecute: Observable.Return(true))
    {
        
    }
}