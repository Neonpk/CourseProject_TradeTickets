using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.AircraftProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AircraftCommands;

public class SaveAircraftDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(AircraftUserViewModel aircraftUserVm)
    {
        return aircraftUserVm.WhenAnyValue(x => x.SelectedAircraft.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync(AircraftUserViewModel aircraftUserVm, IAircraftVmProvider aircraftVmProvider)
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
            
            await aircraftVmProvider.CreateOrEditAircraft(aircraftUserVm.SelectedAircraft);
            aircraftUserVm.SearchAircraftDataCommand.Execute().Subscribe();
        }
        catch (DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            aircraftUserVm.ErrorMessage = pgException.ErrorMessageFromCode(nameof(AircraftUserViewModel));
        }
        catch (DbUpdateException e)
        {
            aircraftUserVm.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        catch (Exception e)
        {
            aircraftUserVm.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            aircraftUserVm.IsLoadingEditMode = false;
            aircraftUserVm.IsLoading = false;
        }
    }

    public SaveAircraftDataCommand(AircraftUserViewModel airuAircraftUserVm, IAircraftVmProvider aircraftVmProvider) :
        base(_ => Observable.Start(async () => await SaveDataAsync(airuAircraftUserVm, aircraftVmProvider)), 
            canExecute: CanExecuteCommand(airuAircraftUserVm).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
