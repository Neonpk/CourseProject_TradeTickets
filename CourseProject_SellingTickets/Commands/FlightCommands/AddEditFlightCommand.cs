using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands;

public class AddEditFlightCommand : ReactiveCommand<bool, Unit>
{

    private static void EditData(FlightUserViewModel flightUserViewModel, IFlightProvider flightDbProvider, bool isNewInstance)
    {
        if (isNewInstance)
            flightUserViewModel.SelectedFlight = new Flight { };
        
        flightUserViewModel.SideBarShowed = true;
    }
    
    public AddEditFlightCommand( FlightUserViewModel flightUserViewModel, IFlightProvider flightProvider ) : 
        base(  isNewInstance => Observable.Start(() => EditData(flightUserViewModel, flightProvider, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }

    public override IObservable<Unit> Execute(bool parameter)
    {
        return base.Execute(parameter);
    }
}