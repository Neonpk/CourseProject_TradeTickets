using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PlaceCommands;

public class AddEditPlaceDataCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(PlaceUserViewModel placeUserVm, bool isNewInstance)
    {
        if (isNewInstance)
            placeUserVm.SelectedPlace = new Place();

        placeUserVm.SideBarShowed = true;
    }
    
    public AddEditPlaceDataCommand( PlaceUserViewModel placeUserVm ) : 
        base(  isNewInstance => Observable.Start(() => EditData(placeUserVm, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}
