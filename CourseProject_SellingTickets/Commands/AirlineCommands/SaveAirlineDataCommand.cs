using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.AirlineProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AirlineCommands;

public class SaveAirlineDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(AirlineUserViewModel airlineUserVm)
    {
        return airlineUserVm.WhenAnyValue(x => x.SelectedAirline.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync(AirlineUserViewModel airlineUserVm, IAirlineVmProvider airlineVmProvider)
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

            await airlineVmProvider.CreateOrEditAirline(airlineUserVm.SelectedAirline);
            airlineUserVm.SearchAirlineDataCommand.Execute().Subscribe();
        }
        catch (DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            airlineUserVm.ErrorMessage = pgException.ErrorMessageFromCode(nameof(AirlineUserViewModel));
        }
        catch (DbUpdateException e)
        {
            airlineUserVm.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            airlineUserVm.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            airlineUserVm.IsLoadingEditMode = false;
            airlineUserVm.IsLoading = false;
        }
    }
    
    public SaveAirlineDataCommand(AirlineUserViewModel airlineUserVm, IAirlineVmProvider airlineVmProvider) : 
        base(_ => Observable.Start(async () => await SaveDataAsync(airlineUserVm, airlineVmProvider)), 
        canExecute: CanExecuteCommand(airlineUserVm).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
