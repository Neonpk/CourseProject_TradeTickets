using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.PhotoProvider;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PhotoCommands;

public class DeletePhotoDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(PhotoUserViewModel photoUserViewModel, IPhotoVmProvider? photoVmProvider, IConnectionStateProvider connectionStateProvider)
    {
        photoUserViewModel.ErrorMessage = string.Empty;
        photoUserViewModel.IsLoadingEditMode = true;
        
        photoUserViewModel.DatabaseHasConnected = await connectionStateProvider.IsConnected();
        
        if (!photoUserViewModel.DatabaseHasConnected)
        {
            photoUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            photoUserViewModel.IsLoadingEditMode = false;
            return;
        }
        
        try
        {
            Photo selectedPhoto = photoUserViewModel.SelectedPhoto;
            
            var dbState = await photoVmProvider!.DeletePhoto(selectedPhoto);

            photoUserViewModel.SearchPhotoDataCommand?.Execute().Subscribe();
        }
        catch (Exception e)
        {
            photoUserViewModel.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }

        photoUserViewModel.IsLoadingEditMode = false;
    }
    
    public DeletePhotoDataCommand(PhotoUserViewModel photoUserViewModel, IPhotoVmProvider? photoVmProvider, IConnectionStateProvider connectionStateProvider) : 
        base(_ => Observable.Start(async () => await DeleteDataAsync(photoUserViewModel, photoVmProvider, connectionStateProvider)), 
            canExecute: Observable.Return(true)) 
    {
    }
}