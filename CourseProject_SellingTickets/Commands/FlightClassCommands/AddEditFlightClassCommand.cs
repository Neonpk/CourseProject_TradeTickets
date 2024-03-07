using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightClassCommands;

public class AddEditFlightClassCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(FlightClassUserViewModel flightClassUserViewModel, bool isNewInstance)
    {
        if (isNewInstance)
            flightClassUserViewModel.SelectedFlightClass = new FlightClass();

        flightClassUserViewModel.SideBarShowed = true;
    }
    
    public AddEditFlightClassCommand(FlightClassUserViewModel flightClassUserViewModel) : 
        base(  isNewInstance => Observable.Start(() => EditData(flightClassUserViewModel, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}