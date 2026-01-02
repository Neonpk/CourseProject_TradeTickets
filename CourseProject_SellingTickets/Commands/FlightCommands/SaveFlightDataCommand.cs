using System;
using System.Reactive;
using System.Reactive.Linq;
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
    private static IObservable<bool> CanExecuteCommand(FlightUserViewModel flightVm)
    {
        return flightVm.WhenAnyValue(x => x.SelectedFlight.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync( FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider)
    {
        flightUserViewModel.ErrorMessage = string.Empty;
        flightUserViewModel.IsLoadingEditMode = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        if (!flightUserViewModel.DatabaseHasConnected)
        {
            flightUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            flightUserViewModel.IsLoadingEditMode = false;
            return;
        }
        
        try
        {
            Flight selectedFlight = flightUserViewModel.SelectedFlight;
            var dbState = await flightVmProvider.CreateOrEditFlight(selectedFlight);

            flightUserViewModel.SearchFlightDataCommand!.Execute().Subscribe();
        }
        catch (DbUpdateException e) when ( e.InnerException is NpgsqlException pgException )
        {
            flightUserViewModel.ErrorMessage = pgException.ErrorMessageFromCode(nameof(FlightUserViewModel));
        }
        catch (DbUpdateException e)
        {
            flightUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        catch (Exception e)
        {
            flightUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        
        flightUserViewModel.IsLoadingEditMode = false;
    }

    public SaveFlightDataCommand(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider) :
        base(_ => Observable.Start(async () => await SaveDataAsync(flightUserViewModel, flightVmProvider)), 
            canExecute: CanExecuteCommand(flightUserViewModel).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
