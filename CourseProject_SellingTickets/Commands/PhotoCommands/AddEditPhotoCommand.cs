using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PhotoCommands;

public class AddEditPhotoCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(PhotoUserViewModel photoUserVm, bool isNewInstance)
    {
        if (isNewInstance)
            photoUserVm.SelectedPhoto = new Photo();

        photoUserVm.SideBarShowed = true;
    }
    
    public AddEditPhotoCommand(PhotoUserViewModel photoUserVm) : 
        base(  isNewInstance => Observable.Start(() => EditData(photoUserVm, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}
