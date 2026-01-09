using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Interfaces.TicketProviderInterface;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class BookTicketCommand : ReactiveCommand<Unit, Unit>
{
    private static void BookTicket(TicketUserViewModel ticketUserViewModel, ITicketVmProvider ticketVmProvider)
    {
       // TODO: Finalize 
    }
    
    public BookTicketCommand(TicketUserViewModel ticketUserViewModel, ITicketVmProvider ticketVmProvider) : 
        base( _ => Observable.Start(() => BookTicket(ticketUserViewModel, ticketVmProvider)), canExecute: Observable.Return(true)  )
    {
        
    }
}
