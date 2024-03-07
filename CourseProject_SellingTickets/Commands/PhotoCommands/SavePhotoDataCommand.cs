using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.PhotoProvider;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PhotoCommands;

public class SavePhotoDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(PhotoUserViewModel photoVm)
    {
        return photoVm.WhenAnyValue(x => x.SelectedPhoto.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync( PhotoUserViewModel photoUserViewModel, IPhotoVmProvider photoVmProvider, IConnectionStateProvider connectionStateProvider )
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
            var dbState = await photoVmProvider.CreateOrEditPhoto(selectedPhoto);

            photoUserViewModel.SearchPhotoDataCommand.Execute().Subscribe();
        }
        catch(DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            photoUserViewModel.ErrorMessage = pgException.ErrorMessageFromCode(nameof(PhotoUserViewModel));
        }
        catch (DbUpdateException e)
        {
            photoUserViewModel.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            photoUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        
        photoUserViewModel.IsLoadingEditMode = false;
    }
    
    public SavePhotoDataCommand(PhotoUserViewModel photoUserViewModel, IPhotoVmProvider photoVmProvider, IConnectionStateProvider connectionStateProvider) : 
        base(_ => Observable.Start(async () => await SaveDataAsync(photoUserViewModel, photoVmProvider, connectionStateProvider)), 
        canExecute: CanExecuteCommand(photoUserViewModel).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}