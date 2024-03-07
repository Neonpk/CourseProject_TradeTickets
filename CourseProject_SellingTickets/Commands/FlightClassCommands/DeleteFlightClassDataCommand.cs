using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.FlightClassProvider;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightClassCommands;

public class DeleteFlightClassDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(FlightClassUserViewModel flightClassUserViewModel, IFlightClassVmProvider? flightClassVmProvider, IConnectionStateProvider connectionStateProvider)
    {
        flightClassUserViewModel.ErrorMessage = string.Empty;
        flightClassUserViewModel.IsLoadingEditMode = true;
        
        flightClassUserViewModel.DatabaseHasConnected = await connectionStateProvider.IsConnected();
        
        if (!flightClassUserViewModel.DatabaseHasConnected)
        {
            flightClassUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            flightClassUserViewModel.IsLoadingEditMode = false;
            return;
        }
        
        try
        {
            FlightClass selectedFlightClass = flightClassUserViewModel.SelectedFlightClass;
            
            var dbState = await flightClassVmProvider!.DeleteFlightClass(selectedFlightClass);

            flightClassUserViewModel.SearchFlightClassDataCommand?.Execute().Subscribe();
        }
        catch (Exception e)
        {
            flightClassUserViewModel.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }

        flightClassUserViewModel.IsLoadingEditMode = false;
    }
    
    public DeleteFlightClassDataCommand(FlightClassUserViewModel flightClassUserViewModel, IFlightClassVmProvider? flightClassVmProvider, IConnectionStateProvider connectionStateProvider) : 
        base(_ => Observable.Start(async () => await DeleteDataAsync(flightClassUserViewModel, flightClassVmProvider, connectionStateProvider)), 
            canExecute: Observable.Return(true)) 
    {
    }
}