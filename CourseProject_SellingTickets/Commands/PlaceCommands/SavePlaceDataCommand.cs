using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.PlaceProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PlaceCommands;

public class SavePlaceDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(PlaceUserViewModel placeUserVm)
    {
        return placeUserVm.WhenAnyValue(x => x.SelectedPlace.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync(PlaceUserViewModel placeUserVm, IPlaceVmProvider placeVmProvider )
    {
        try
        {
            placeUserVm.ErrorMessage = string.Empty;
            placeUserVm.IsLoadingEditMode = true;
            placeUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();
        
            if (!await isConnected)
            {
                placeUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }
            
            await placeVmProvider.CreateOrEditPlace(placeUserVm.SelectedPlace);
            placeUserVm.SearchPlaceDataCommand.Execute().Subscribe();
        }
        catch (DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            placeUserVm.ErrorMessage = pgException.ErrorMessageFromCode(nameof(PlaceUserViewModel));
        }
        catch (DbUpdateException e)
        {
            placeUserVm.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        catch (Exception e)
        {
            placeUserVm.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            placeUserVm.IsLoadingEditMode = false;
            placeUserVm.IsLoading = false;
        }
    }

    public SavePlaceDataCommand(PlaceUserViewModel placeUserVm, IPlaceVmProvider placeVmProvider) :
        base(_ => Observable.Start(async () => await SaveDataAsync(placeUserVm, placeVmProvider)), 
            canExecute: CanExecuteCommand(placeUserVm).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
