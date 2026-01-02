using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Interfaces.AircraftProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AircraftCommands;

public class LoadAircraftDataCommand : ReactiveCommand<IEnumerable<Aircraft>, Task>
{
    private static async Task LoadDataAsync(AircraftUserViewModel aircraftUserViewModel, IAircraftVmProvider aircraftVmProvider, IEnumerable<Aircraft> filteredAircrafts)
    {
        var limitRows = aircraftUserViewModel.LimitRows;
        
        aircraftUserViewModel.ErrorMessage = string.Empty;
        aircraftUserViewModel.IsLoading = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
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
    
    public LoadAircraftDataCommand(AircraftUserViewModel aircraftUserViewModel, IAircraftVmProvider aircraftVmProvider) :
        base(fileteredAircrafts => 
                Observable.Start( async () => await LoadDataAsync(aircraftUserViewModel, aircraftVmProvider, fileteredAircrafts)),
            canExecute: Observable.Return(true))
    {
        
    }
}