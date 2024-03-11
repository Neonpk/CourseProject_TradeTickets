using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AircraftCommands;

public class SaveAircraftDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(AircraftUserViewModel flightVm)
    {
        return flightVm.WhenAnyValue(x => x.SelectedAircraft.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync( AircraftUserViewModel aircraftUserViewModel, IAircraftVmProvider aircraftVmProvider)
    {
        aircraftUserViewModel.ErrorMessage = string.Empty;
        aircraftUserViewModel.IsLoadingEditMode = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        if (!aircraftUserViewModel.DatabaseHasConnected)
        {
            aircraftUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            aircraftUserViewModel.IsLoadingEditMode = false;
            return;
        }
        
        try
        {
            Aircraft selectedAircraft = aircraftUserViewModel.SelectedAircraft;
            var dbState = await aircraftVmProvider.CreateOrEditAircraft(selectedAircraft);

            aircraftUserViewModel.SearchAircraftDataCommand!.Execute().Subscribe();
        }
        catch (DbUpdateException e) when ( e.InnerException is NpgsqlException pgException )
        {
            aircraftUserViewModel.ErrorMessage = pgException.ErrorMessageFromCode(nameof(AircraftUserViewModel));
        }
        catch (DbUpdateException e)
        {
            aircraftUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        catch (Exception e)
        {
            aircraftUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        
        aircraftUserViewModel.IsLoadingEditMode = false;
    }

    public SaveAircraftDataCommand(AircraftUserViewModel aircraftUserViewModel, IAircraftVmProvider aircraftVmProvider) :
        base(_ => Observable.Start(async () => await SaveDataAsync(aircraftUserViewModel, aircraftVmProvider)), 
            canExecute: CanExecuteCommand(aircraftUserViewModel).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}