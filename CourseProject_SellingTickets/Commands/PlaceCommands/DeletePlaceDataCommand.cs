using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.PlaceProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PlaceCommands;

public class DeletePlaceDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(PlaceUserViewModel placeUserVm, IPlaceVmProvider? placeVmProvider)
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
            
            await placeVmProvider!.DeletePlace(placeUserVm.SelectedPlace);
            placeUserVm.SearchPlaceDataCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            placeUserVm.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            placeUserVm.IsLoadingEditMode = false;
            placeUserVm.IsLoading = false;
        }
    }

    public DeletePlaceDataCommand(PlaceUserViewModel placeUserVm, IPlaceVmProvider? placeVmProvider) : 
        base(_ => 
            Observable.Start(async () => await DeleteDataAsync(placeUserVm, placeVmProvider)), canExecute: Observable.Return(true))
    {
    }
}
