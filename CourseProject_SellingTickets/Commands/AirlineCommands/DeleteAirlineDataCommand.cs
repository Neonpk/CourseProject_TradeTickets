using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.AirlineProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AirlineCommands;

public class DeleteAirlineDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(AirlineUserViewModel airlineUserViewModel, IAirlineVmProvider? airlineVmProvider)
    {
        airlineUserViewModel.ErrorMessage = string.Empty;
        airlineUserViewModel.IsLoadingEditMode = true;
        
        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        if (!airlineUserViewModel.DatabaseHasConnected)
        {
            airlineUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            airlineUserViewModel.IsLoadingEditMode = false;
            return;
        }
        
        try
        {
            Airline selectedAirline = airlineUserViewModel.SelectedAirline;
            
            var dbState = await airlineVmProvider!.DeleteAirline(selectedAirline);

            airlineUserViewModel.SearchAirlineDataCommand?.Execute().Subscribe();
        }
        catch (Exception e)
        {
            airlineUserViewModel.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }

        airlineUserViewModel.IsLoadingEditMode = false;
    }
    
    public DeleteAirlineDataCommand(AirlineUserViewModel airlineUserViewModel, IAirlineVmProvider? airlineVmProvider) : 
        base(_ => Observable.Start(async () => await DeleteDataAsync(airlineUserViewModel, airlineVmProvider)), 
            canExecute: Observable.Return(true)) 
    {
    }
}
