using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightCommands;

public class AddEditFlightCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(FlightUserViewModel flightUserVm, bool isNewInstance)
    {
        if (isNewInstance)
            flightUserVm.SelectedFlight = new Flight();

        flightUserVm.SideBarShowed = true;
    }
    
    public AddEditFlightCommand(FlightUserViewModel flightUserVm ) : 
        base(  isNewInstance => Observable.Start(() => EditData(flightUserVm, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}
