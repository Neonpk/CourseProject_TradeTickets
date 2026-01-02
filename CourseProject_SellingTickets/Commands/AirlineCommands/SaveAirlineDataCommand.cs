using System;
using System.Reactive;
using System.Reactive.Linq;
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
    private static IObservable<bool> CanExecuteCommand(AirlineUserViewModel airlineUserViewModel)
    {
        return airlineUserViewModel.WhenAnyValue(x => x.SelectedAirline.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync( AirlineUserViewModel airlineUserViewModel, IAirlineVmProvider airlineVmProvider)
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
            var dbState = await airlineVmProvider.CreateOrEditAirline(selectedAirline);

            airlineUserViewModel.SearchAirlineDataCommand.Execute().Subscribe();
        }
        catch(DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            airlineUserViewModel.ErrorMessage = pgException.ErrorMessageFromCode(nameof(AirlineUserViewModel));
        }
        catch (DbUpdateException e)
        {
            airlineUserViewModel.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            airlineUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        
        airlineUserViewModel.IsLoadingEditMode = false;
    }
    
    public SaveAirlineDataCommand(AirlineUserViewModel airlineUserViewModel, IAirlineVmProvider airlineVmProvider) : 
        base(_ => Observable.Start(async () => await SaveDataAsync(airlineUserViewModel, airlineVmProvider)), 
        canExecute: CanExecuteCommand(airlineUserViewModel).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
