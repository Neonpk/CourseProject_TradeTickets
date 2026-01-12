using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Interfaces.Common;
using CourseProject_SellingTickets.Interfaces.TicketProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class CancelTicketCommand : ReactiveCommand<Unit, Task>
{
    private static async Task CancelTicket(TicketUserViewModel ticketUserVm, ITicketVmProvider ticketVmProvider)
    {
        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        ticketUserVm.ErrorMessage = String.Empty;

        Int64 ticketId = ticketUserVm.SelectedTicket.Id;

        ticketUserVm.IsLoading = true;
        IResult<string> result = await ticketVmProvider.CancelTicket(ticketId);
        ticketUserVm.IsLoading = false;

        if (result.IsSuccess)
        {
            ticketUserVm.SearchTicketDataCommand.Execute().Subscribe();
        }
        else
        {
            ticketUserVm.ErrorMessage = $"Не удалось отменить билет на рейс: {result.Message}.";
        }
    }
    
    public CancelTicketCommand(TicketUserViewModel ticketUserVm, ITicketVmProvider ticketVmProvider) : 
        base( _ => Observable.Start(async () => await CancelTicket(ticketUserVm, ticketVmProvider)), 
            canExecute: Observable.Return(true) )
    {
        
    }
}
