using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
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
    private static async Task LoadDataAsync(AircraftUserViewModel aircraftUserVm, IAircraftVmProvider aircraftVmProvider, IEnumerable<Aircraft> filteredAircrafts)
    {
        try
        {
            var limitRows = aircraftUserVm.LimitRows;

            aircraftUserVm.ErrorMessage = string.Empty;
            aircraftUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();

            if (!await isConnected)
            {
                aircraftUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }

            bool hasSearching = aircraftUserVm.HasSearching;

            IEnumerable<Aircraft> aircrafts =
                hasSearching ? filteredAircrafts : await aircraftVmProvider.GetTopAircrafts(limitRows);

            IEnumerable<Photo> photos = await aircraftVmProvider.GetAllPhotos();

            Dispatcher.UIThread.Post(() =>
            {
                aircraftUserVm.Photos.Clear();
                aircraftUserVm.Photos.AddRange(photos);

                aircraftUserVm.AircraftItems.Clear();
                aircraftUserVm.AircraftItems.AddRange(aircrafts);

            });

            aircraftUserVm.SortAircraftCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            aircraftUserVm.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
        finally
        {
            aircraftUserVm.IsLoading = false;
        }
    }
    
    public LoadAircraftDataCommand(AircraftUserViewModel aircraftUserVm, IAircraftVmProvider aircraftVmProvider) :
        base(fileteredAircrafts => 
                Observable.Start( async () => await LoadDataAsync(aircraftUserVm, aircraftVmProvider, fileteredAircrafts)),
            canExecute: Observable.Return(true))
    {
        
    }
}
