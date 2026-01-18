using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.AirlineProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AirlineCommands;

public class DeleteAirlineDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(AirlineUserViewModel airlineUserVm, IAirlineVmProvider? airlineVmProvider)
    {
        try
        {
            airlineUserVm.ErrorMessage = string.Empty;
            airlineUserVm.IsLoadingEditMode = true;
            airlineUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();

            if (!await isConnected)
            {
                airlineUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }

            await airlineVmProvider!.DeleteAirline(airlineUserVm.SelectedAirline);
            airlineUserVm.SearchAirlineDataCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            airlineUserVm.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            airlineUserVm.IsLoadingEditMode = false;
            airlineUserVm.IsLoading = false;
        }
    }
    
    public DeleteAirlineDataCommand(AirlineUserViewModel airlineUserVm, IAirlineVmProvider? airlineVmProvider) : 
        base(_ => Observable.Start(async () => await DeleteDataAsync(airlineUserVm, airlineVmProvider)), 
            canExecute: Observable.Return(true)) 
    {
    }
}
