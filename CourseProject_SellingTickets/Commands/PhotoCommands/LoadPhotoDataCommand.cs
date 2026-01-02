using System;
using System.Collections.Generic;
using System.Reactive.Linq;
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
    private static async Task LoadDataAsync(PhotoUserViewModel photoUserViewModel, IPhotoVmProvider photoVmProvider, IEnumerable<Photo> filteredPhotos)
    {
        var limitRows = photoUserViewModel.LimitRows;

        photoUserViewModel.ErrorMessage = string.Empty;
        photoUserViewModel.IsLoading = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        try
        {
            bool hasSearching = photoUserViewModel.HasSearching;

            IEnumerable<Photo> photos =
                hasSearching ? filteredPhotos! : await photoVmProvider.GetTopPhotos(limitRows);

            Dispatcher.UIThread.Post(() =>
            {
                photoUserViewModel.PhotoItems.Clear();
                photoUserViewModel.PhotoItems.AddRange(photos);
            });
            
        }
        catch (Exception e)
        {
            photoUserViewModel.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }

        photoUserViewModel.IsLoading = false;
    }
    
    public LoadPhotoDataCommand(PhotoUserViewModel photoUserViewModel, IPhotoVmProvider photoVmProvider) :
        base(filteredPhotos => Observable.Start(async () => 
                await LoadDataAsync(photoUserViewModel, photoVmProvider, filteredPhotos) ),
            canExecute: Observable.Return(true))
    {
    }
}
