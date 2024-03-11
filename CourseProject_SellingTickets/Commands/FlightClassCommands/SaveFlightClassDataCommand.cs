using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.FlightClassProvider;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightClassCommands;

public class SaveFlightClassDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(FlightClassUserViewModel flightClassVm)
    {
        return flightClassVm.WhenAnyValue(x => x.SelectedFlightClass.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync( FlightClassUserViewModel flightClassUserViewModel, IFlightClassVmProvider flightClassVmProvider)
    {
        flightClassUserViewModel.ErrorMessage = string.Empty;
        flightClassUserViewModel.IsLoadingEditMode = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        if (!flightClassUserViewModel.DatabaseHasConnected)
        {
            flightClassUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            flightClassUserViewModel.IsLoadingEditMode = false;
            return;
        }

        try
        {
            FlightClass selectedFlightClass = flightClassUserViewModel.SelectedFlightClass;
            var dbState = await flightClassVmProvider.CreateOrEditFlightClass(selectedFlightClass);

            flightClassUserViewModel.SearchFlightClassDataCommand.Execute().Subscribe();
        }
        catch(DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            flightClassUserViewModel.ErrorMessage = pgException.ErrorMessageFromCode(nameof(FlightClassUserViewModel));
        }
        catch (DbUpdateException e)
        {
            flightClassUserViewModel.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            flightClassUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        
        flightClassUserViewModel.IsLoadingEditMode = false;
    }
    
    public SaveFlightClassDataCommand(FlightClassUserViewModel flightClassUserViewModel, IFlightClassVmProvider flightClassVmProvider) : 
        base(_ => Observable.Start(async () => await SaveDataAsync(flightClassUserViewModel, flightClassVmProvider)), 
        canExecute: CanExecuteCommand(flightClassUserViewModel).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}