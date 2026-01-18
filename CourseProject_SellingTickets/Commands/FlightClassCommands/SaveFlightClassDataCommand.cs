using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.FlightClassProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightClassCommands;

public class SaveFlightClassDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(FlightClassUserViewModel flightClassUserVm)
    {
        return flightClassUserVm.WhenAnyValue(x => x.SelectedFlightClass.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync( FlightClassUserViewModel flightClassUserVm, IFlightClassVmProvider flightClassVmProvider)
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
            
            await flightClassVmProvider.CreateOrEditFlightClass(flightClassUserVm.SelectedFlightClass);
            flightClassUserVm.SearchFlightClassDataCommand.Execute().Subscribe();
        }
        catch (DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            flightClassUserVm.ErrorMessage = pgException.ErrorMessageFromCode(nameof(FlightClassUserViewModel));
        }
        catch (DbUpdateException e)
        {
            flightClassUserVm.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            flightClassUserVm.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            flightClassUserVm.IsLoadingEditMode = false;
            flightClassUserVm.IsLoading = false;
        }
    }
    
    public SaveFlightClassDataCommand(FlightClassUserViewModel flightClassUserVm, IFlightClassVmProvider flightClassVmProvider) : 
        base(_ => Observable.Start(async () => await SaveDataAsync(flightClassUserVm, flightClassVmProvider)), 
        canExecute: CanExecuteCommand(flightClassUserVm).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
