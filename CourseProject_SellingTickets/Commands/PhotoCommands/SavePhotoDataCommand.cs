using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.PhotoProviderInterface;
using CourseProject_SellingTickets.Models;
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
    
    private static async Task SaveDataAsync( PhotoUserViewModel photoUserVm, IPhotoVmProvider photoVmProvider)
    {
        try
        {
            photoUserVm.ErrorMessage = string.Empty;
            photoUserVm.IsLoading = true;
            photoUserVm.IsLoadingEditMode = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();
        
            if (!await isConnected)
            {
                photoUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }
            
            Photo photo = photoUserVm.SelectedPhoto;

            if (photo.SelectedFilePhoto is not null)
            {
                var freeImageResult = await photoUserVm.GenerateFreeImageCommand.Execute(
                    photo.SelectedFilePhoto.GetValueOrDefault(new FileMeta())
                ).ToTask().Unwrap();

                if (!freeImageResult.IsSuccess)
                {
                    photoUserVm.ErrorMessage = freeImageResult.Message!;
                    return;
                }
                
                photo.UrlPath = freeImageResult.Value!;
            }
            
            await photoVmProvider.CreateOrEditPhoto(photo);
            photoUserVm.SearchPhotoDataCommand.Execute().Subscribe();
        }
        catch (DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            photoUserVm.ErrorMessage = pgException.ErrorMessageFromCode(nameof(PhotoUserViewModel));
        }
        catch (DbUpdateException e)
        {
            photoUserVm.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            photoUserVm.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            photoUserVm.IsLoading = false;
            photoUserVm.IsLoadingEditMode = false;   
        }
    }
    
    public SavePhotoDataCommand(PhotoUserViewModel photoUserVm, IPhotoVmProvider photoVmProvider) : 
        base(_ => Observable.Start(async () => await SaveDataAsync(photoUserVm, photoVmProvider)), 
        canExecute: CanExecuteCommand(photoUserVm).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
