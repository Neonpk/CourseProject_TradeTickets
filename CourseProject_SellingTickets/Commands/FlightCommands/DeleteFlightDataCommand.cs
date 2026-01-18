using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.FlightProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightCommands;

public class DeleteFlightDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(FlightUserViewModel flightUserVm, IFlightVmProvider? flightProvider)
    {
        try
        {
            flightUserVm.ErrorMessage = string.Empty;
            flightUserVm.IsLoadingEditMode = true;
            flightUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();
        
            if (!await isConnected)
            {
                flightUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }

            await flightProvider!.DeleteFlight(flightUserVm.SelectedFlight);
            flightUserVm.SearchFlightDataCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            flightUserVm.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            flightUserVm.IsLoadingEditMode = false;
            flightUserVm.IsLoading = false;
        }
    }

    public DeleteFlightDataCommand(FlightUserViewModel flightUserVm, IFlightVmProvider? flightProvider) : 
        base(_ => 
            Observable.Start(async () => await DeleteDataAsync(flightUserVm, flightProvider)), canExecute: Observable.Return(true))
    {
    }
}
