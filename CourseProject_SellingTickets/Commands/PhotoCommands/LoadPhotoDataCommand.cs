using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Interfaces.PhotoProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PhotoCommands;

public class LoadPhotoDataCommand : ReactiveCommand<IEnumerable<Photo>, Task>
{
    private static async Task LoadDataAsync(PhotoUserViewModel photoUserVm, IPhotoVmProvider photoVmProvider, IEnumerable<Photo> filteredPhotos)
    {
        try
        {
            var limitRows = photoUserVm.LimitRows;

            photoUserVm.ErrorMessage = string.Empty;
            photoUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();

            if (!await isConnected)
            {
                photoUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }
            
            bool hasSearching = photoUserVm.HasSearching;

            IEnumerable<Photo> photos =
                hasSearching ? filteredPhotos : await photoVmProvider.GetTopPhotos(limitRows);

            Dispatcher.UIThread.Post(() =>
            {
                photoUserVm.PhotoItems.Clear();
                photoUserVm.PhotoItems.AddRange(photos);
            });

        }
        catch (Exception e)
        {
            photoUserVm.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
        finally
        {
            photoUserVm.IsLoading = false;   
        }
    }
    
    public LoadPhotoDataCommand(PhotoUserViewModel photoUserVm, IPhotoVmProvider photoVmProvider) :
        base(filteredPhotos => Observable.Start(async () => 
                await LoadDataAsync(photoUserVm, photoVmProvider, filteredPhotos) ),
            canExecute: Observable.Return(true))
    {
    }
}
