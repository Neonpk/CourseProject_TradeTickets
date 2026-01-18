using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.PhotoProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PhotoCommands;

public class DeletePhotoDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(PhotoUserViewModel photoUserVm, IPhotoVmProvider? photoVmProvider)
    {
        try
        {
            photoUserVm.ErrorMessage = string.Empty;
            photoUserVm.IsLoadingEditMode = true;
            photoUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();
        
            if (!await isConnected)
            {
                photoUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }
            
            await photoVmProvider!.DeletePhoto(photoUserVm.SelectedPhoto);
            photoUserVm.SearchPhotoDataCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            photoUserVm.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            photoUserVm.IsLoadingEditMode = false;
            photoUserVm.IsLoading = false;
        }
    }
    
    public DeletePhotoDataCommand(PhotoUserViewModel photoUserVm, IPhotoVmProvider? photoVmProvider) : 
        base(_ => Observable.Start(async () => await DeleteDataAsync(photoUserVm, photoVmProvider)), 
            canExecute: Observable.Return(true)) 
    {
    }
}
