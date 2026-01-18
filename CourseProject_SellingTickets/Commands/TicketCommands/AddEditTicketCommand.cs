using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class AddEditTicketCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(TicketUserViewModel ticketUserVm, bool isNewInstance)
    {
        if (isNewInstance)
            ticketUserVm.SelectedTicket = new Ticket();

        ticketUserVm.SideBarShowed = true;
    }
    
    public AddEditTicketCommand(TicketUserViewModel ticketUserVm) : 
        base(  isNewInstance => Observable.Start(() => EditData(ticketUserVm, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}
