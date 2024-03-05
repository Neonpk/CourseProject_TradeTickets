using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.PlaceProvider;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PlaceCommands;

public class SavePlaceDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(PlaceUserViewModel flightVm)
    {
        return flightVm.WhenAnyValue(x => x.SelectedPlace.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync( PlaceUserViewModel placeUserViewModel, IPlaceVmProvider placeVmProvider, IConnectionStateProvider connectionStateProvider )
    {
        placeUserViewModel.ErrorMessage = string.Empty;
        placeUserViewModel.IsLoadingEditMode = true;

        placeUserViewModel.DatabaseHasConnected = await connectionStateProvider.IsConnected();
        
        if (!placeUserViewModel.DatabaseHasConnected)
        {
            placeUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            placeUserViewModel.IsLoadingEditMode = false;
            return;
        }
        
        try
        {
            Place selectedAircraft = placeUserViewModel.SelectedPlace;
            var dbState = await placeVmProvider.CreateOrEditPlace(selectedAircraft);

            placeUserViewModel.SearchPlaceDataCommand!.Execute().Subscribe();
        }
        catch (DbUpdateException e) when ( e.InnerException is NpgsqlException pgException )
        {
            placeUserViewModel.ErrorMessage = pgException.ErrorMessageFromCode(nameof(PlaceUserViewModel));
        }
        catch (DbUpdateException e)
        {
            placeUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        catch (Exception e)
        {
            placeUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        
        placeUserViewModel.IsLoadingEditMode = false;
    }

    public SavePlaceDataCommand(PlaceUserViewModel placeUserViewModel, IPlaceVmProvider placeVmProvider, IConnectionStateProvider connectionStateProvider) :
        base(_ => Observable.Start(async () => await SaveDataAsync(placeUserViewModel, placeVmProvider, connectionStateProvider)), 
            canExecute: CanExecuteCommand(placeUserViewModel).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}