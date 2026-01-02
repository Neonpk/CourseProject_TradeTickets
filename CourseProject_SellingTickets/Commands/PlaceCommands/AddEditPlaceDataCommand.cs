using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.PlaceCommands;

public class AddEditPlaceDataCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(PlaceUserViewModel placeUserViewModel, bool isNewInstance)
    {
        if (isNewInstance)
            placeUserViewModel.SelectedPlace = new Place();

        placeUserViewModel.SideBarShowed = true;
    }
    
    public AddEditPlaceDataCommand( PlaceUserViewModel placeUserViewModel ) : 
        base(  isNewInstance => Observable.Start(() => EditData(placeUserViewModel, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}
