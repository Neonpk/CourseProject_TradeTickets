using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.AircraftProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AircraftCommands;

public class DeleteAircraftDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(AircraftUserViewModel aircraftUserVm, IAircraftVmProvider? aircraftVmProvider)
    {
        try
        {
            aircraftUserVm.ErrorMessage = string.Empty;
            aircraftUserVm.IsLoadingEditMode = true;
            aircraftUserVm.IsLoading = true;
            
            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();

            if (!await isConnected)
            {
                aircraftUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }

            await aircraftVmProvider!.DeleteAircraft(aircraftUserVm.SelectedAircraft);
            aircraftUserVm.SearchAircraftDataCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            aircraftUserVm.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            aircraftUserVm.IsLoadingEditMode = false;
            aircraftUserVm.IsLoading = false;
        }
    }

    public DeleteAircraftDataCommand( AircraftUserViewModel aircraftUserVm, IAircraftVmProvider? aircraftVmProvider) : 
        base(_ => 
            Observable.Start(async () => await DeleteDataAsync(aircraftUserVm, aircraftVmProvider)), canExecute: Observable.Return(true))
    {
        
    }
}
