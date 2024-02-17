using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class AddEditTicketCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(TicketUserViewModel ticketUserViewModel, bool isNewInstance)
    {
        if (isNewInstance)
            ticketUserViewModel.SelectedTicket = new Ticket();

        ticketUserViewModel.SideBarShowed = true;
    }
    
    public AddEditTicketCommand(TicketUserViewModel ticketUserViewModel) : 
        base(  isNewInstance => Observable.Start(() => EditData(ticketUserViewModel, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}