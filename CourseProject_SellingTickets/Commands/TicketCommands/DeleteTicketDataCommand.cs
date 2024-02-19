using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.TicketProvider;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class DeleteTicketDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(TicketUserViewModel ticketUserViewModel, ITicketVmProvider? flightProvider, IConnectionStateProvider connectionStateProvider)
    {
        ticketUserViewModel.ErrorMessage = string.Empty;
        ticketUserViewModel.IsLoadingEditMode = true;
        
        ticketUserViewModel.DatabaseHasConnected = await connectionStateProvider.IsConnected();
        
        if (!ticketUserViewModel.DatabaseHasConnected)
        {
            ticketUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            ticketUserViewModel.IsLoadingEditMode = false;
            return;
        }
        
        try
        {
            Ticket selectedTicket = ticketUserViewModel.SelectedTicket;
            
            bool isDeleted = await flightProvider!.DeleteTicket(selectedTicket);

            ticketUserViewModel.SearchTicketDataCommand?.Execute().Subscribe();
        }
        catch (Exception e)
        {
            ticketUserViewModel.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.InnerException!.Message})";
        }

        ticketUserViewModel.IsLoadingEditMode = false;
    }
    
    public DeleteTicketDataCommand(TicketUserViewModel ticketUserViewModel, ITicketVmProvider? ticketVmProvider, IConnectionStateProvider connectionStateProvider) : 
        base(_ => Observable.Start(async () => await DeleteDataAsync(ticketUserViewModel, ticketVmProvider, connectionStateProvider)), 
            canExecute: Observable.Return(true)) 
    {
    }
}