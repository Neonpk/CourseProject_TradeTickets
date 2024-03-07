using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PhotoCommands;

public class AddEditPhotoCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(PhotoUserViewModel photoUserViewModel, bool isNewInstance)
    {
        if (isNewInstance)
            photoUserViewModel.SelectedPhoto = new Photo();

        photoUserViewModel.SideBarShowed = true;
    }
    
    public AddEditPhotoCommand(PhotoUserViewModel photoUserViewModel) : 
        base(  isNewInstance => Observable.Start(() => EditData(photoUserViewModel, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}