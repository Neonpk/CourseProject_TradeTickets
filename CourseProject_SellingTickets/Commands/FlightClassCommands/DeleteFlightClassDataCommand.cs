using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.FlightClassProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightClassCommands;

public class DeleteFlightClassDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(FlightClassUserViewModel flightClassUserVm, IFlightClassVmProvider? flightClassVmProvider)
    {
        try
        {
            flightClassUserVm.ErrorMessage = string.Empty;
            flightClassUserVm.IsLoadingEditMode = true;
            flightClassUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();
        
            if (!await isConnected)
            {
                flightClassUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }
            
            await flightClassVmProvider!.DeleteFlightClass(flightClassUserVm.SelectedFlightClass);
            flightClassUserVm.SearchFlightClassDataCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            flightClassUserVm.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            flightClassUserVm.IsLoadingEditMode = false;
            flightClassUserVm.IsLoading = false;
        }
    }
    
    public DeleteFlightClassDataCommand(FlightClassUserViewModel flightClassUserVm, IFlightClassVmProvider? flightClassVmProvider) : 
        base(_ => Observable.Start(async () => await DeleteDataAsync(flightClassUserVm, flightClassVmProvider)), 
            canExecute: Observable.Return(true)) 
    {
    }
}
