using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AircraftCommands;

public class AddEditAircraftDataCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(AircraftUserViewModel aircraftUserVm, bool isNewInstance)
    {
        if (isNewInstance)
            aircraftUserVm.SelectedAircraft = new Aircraft();

        aircraftUserVm.SideBarShowed = true;
    }
    
    public AddEditAircraftDataCommand( AircraftUserViewModel aircraftUserVm ) : 
        base(  isNewInstance => Observable.Start(() => EditData(aircraftUserVm, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}
