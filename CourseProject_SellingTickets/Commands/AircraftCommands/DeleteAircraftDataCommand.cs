using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.AircraftProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AircraftCommands;

public class DeleteAircraftDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(AircraftUserViewModel aircraftUserViewModel, IAircraftVmProvider? aircraftVmProvider)
    {
        aircraftUserViewModel.ErrorMessage = string.Empty;
        aircraftUserViewModel.IsLoadingEditMode = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        if (!aircraftUserViewModel.DatabaseHasConnected)
        {
            aircraftUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            aircraftUserViewModel.IsLoadingEditMode = false;
            return;
        }
        
        try
        {
            Aircraft? selectedAircraft = aircraftUserViewModel.SelectedAircraft;
            
            var dbState = await aircraftVmProvider!.DeleteAircraft(selectedAircraft);

            aircraftUserViewModel.SearchAircraftDataCommand?.Execute().Subscribe();
        }
        catch (Exception e)
        {
            aircraftUserViewModel.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }

        aircraftUserViewModel.IsLoadingEditMode = false;
    }

    public DeleteAircraftDataCommand( AircraftUserViewModel aircraftUserViewModel, IAircraftVmProvider? aircraftVmProvider) : 
        base(_ => 
            Observable.Start(async () => await DeleteDataAsync(aircraftUserViewModel, aircraftVmProvider)), canExecute: Observable.Return(true))
    {
        
    }
}