using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Interfaces.TicketProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class BookTicketCommand : ReactiveCommand<Unit, Task>
{
    private static async Task BookTicket(TicketUserViewModel ticketUserVm, ITicketVmProvider ticketVmProvider)
    {
        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        ticketUserVm.ErrorMessage = String.Empty;
        
        if (ticketUserVm.TicketUserVmParam == null) return;
        
        Int64 userId = ticketUserVm.TicketUserVmParam.UserId;
        Int64 ticketId = ticketUserVm.SelectedTicket.Id;

        ticketUserVm.IsLoading = true;
        IResult<string> result = await ticketVmProvider.BuyTicket(userId, ticketId);
        ticketUserVm.IsLoading = false;

        if (result.IsSuccess)
        {
            ticketUserVm.SearchTicketDataCommand.Execute().Subscribe();
        }
        else
        {
            ticketUserVm.ErrorMessage = $"Не удалось забронировать билет: {result.Message}.";
        }
    }
    
    public BookTicketCommand(TicketUserViewModel ticketUserVm, ITicketVmProvider ticketVmProvider) : 
        base( _ => Observable.Start(async () => await BookTicket(ticketUserVm, ticketVmProvider)), canExecute: Observable.Return(true)  )
    {
        
    }
}
