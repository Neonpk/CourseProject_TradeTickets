using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.FlightProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightCommands;

public class SaveFlightDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(FlightUserViewModel flightUserVm)
    {
        return flightUserVm.WhenAnyValue(x => x.SelectedFlight.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync(FlightUserViewModel flightUserVm, IFlightVmProvider flightVmProvider)
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
            
            await flightVmProvider.CreateOrEditFlight(flightUserVm.SelectedFlight);
            flightUserVm.SearchFlightDataCommand.Execute().Subscribe();
        }
        catch (DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            flightUserVm.ErrorMessage = pgException.ErrorMessageFromCode(nameof(FlightUserViewModel));
        }
        catch (DbUpdateException e)
        {
            flightUserVm.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        catch (Exception e)
        {
            flightUserVm.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            flightUserVm.IsLoadingEditMode = false;
            flightUserVm.IsLoading = false;
        }
    }

    public SaveFlightDataCommand(FlightUserViewModel flightUserVm, IFlightVmProvider flightVmProvider) :
        base(_ => Observable.Start(async () => await SaveDataAsync(flightUserVm, flightVmProvider)), 
            canExecute: CanExecuteCommand(flightUserVm).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
