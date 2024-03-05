using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AircraftCommands;

public class AddEditAircraftDataCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(AircraftUserViewModel aircraftUserViewModel, bool isNewInstance)
    {
        if (isNewInstance)
            aircraftUserViewModel.SelectedAircraft = new Aircraft();

        aircraftUserViewModel.SideBarShowed = true;
    }
    
    public AddEditAircraftDataCommand( AircraftUserViewModel aircraftUserViewModel ) : 
        base(  isNewInstance => Observable.Start(() => EditData(aircraftUserViewModel, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}