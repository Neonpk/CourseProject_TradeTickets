using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AirlineCommands;

public class AddEditAirlineCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(AirlineUserViewModel airlineUserViewModel, bool isNewInstance)
    {
        if (isNewInstance)
            airlineUserViewModel.SelectedAirline = new Airline();

        airlineUserViewModel.SideBarShowed = true;
    }
    
    public AddEditAirlineCommand(AirlineUserViewModel airlineUserViewModel) : 
        base(  isNewInstance => Observable.Start(() => EditData(airlineUserViewModel, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
    }
}
