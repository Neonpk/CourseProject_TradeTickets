using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.TicketProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class DeleteTicketDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(TicketUserViewModel ticketUserVm, ITicketVmProvider? flightProvider)
    {
        try
        {
            ticketUserVm.ErrorMessage = string.Empty;
            ticketUserVm.IsLoadingEditMode = true;
            ticketUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();
        
            if (!await isConnected)
            {
                ticketUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }
            
            await flightProvider!.DeleteTicket(ticketUserVm.SelectedTicket);
            ticketUserVm.SearchTicketDataCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            ticketUserVm.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            ticketUserVm.IsLoadingEditMode = false;
            ticketUserVm.IsLoading = false;
        }
    }
    
    public DeleteTicketDataCommand(TicketUserViewModel ticketUserVm, ITicketVmProvider? ticketVmProvider) : 
        base(_ => Observable.Start(async () => await DeleteDataAsync(ticketUserVm, ticketVmProvider)), 
            canExecute: Observable.Return(true)) 
    {
    }
}
