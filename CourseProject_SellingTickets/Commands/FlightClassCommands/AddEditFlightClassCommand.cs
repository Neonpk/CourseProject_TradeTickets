using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightClassCommands;

public class AddEditFlightClassCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(FlightClassUserViewModel flightClassUserVm, bool isNewInstance)
    {
        if (isNewInstance)
            flightClassUserVm.SelectedFlightClass = new FlightClass();

        flightClassUserVm.SideBarShowed = true;
    }
    
    public AddEditFlightClassCommand(FlightClassUserViewModel flightClassUserVm) : 
        base(  isNewInstance => Observable.Start(() => EditData(flightClassUserVm, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}
