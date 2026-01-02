using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightCommands;

public class AddEditFlightCommand : ReactiveCommand<bool, Unit>
{

    private static void EditData(FlightUserViewModel flightUserViewModel, bool isNewInstance)
    {
        if (isNewInstance)
            flightUserViewModel.SelectedFlight = new Flight();

        flightUserViewModel.SideBarShowed = true;
    }
    
    public AddEditFlightCommand( FlightUserViewModel flightUserViewModel ) : 
        base(  isNewInstance => Observable.Start(() => EditData(flightUserViewModel, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}
