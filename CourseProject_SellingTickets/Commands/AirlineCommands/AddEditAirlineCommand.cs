using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AirlineCommands;

public class AddEditAirlineCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(AirlineUserViewModel airlineUserVm, bool isNewInstance)
    {
        if (isNewInstance)
            airlineUserVm.SelectedAirline = new Airline();

        airlineUserVm.SideBarShowed = true;
    }
    
    public AddEditAirlineCommand(AirlineUserViewModel airlineUserVm) : 
        base(  isNewInstance => Observable.Start(() => EditData(airlineUserVm, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
    }
}
