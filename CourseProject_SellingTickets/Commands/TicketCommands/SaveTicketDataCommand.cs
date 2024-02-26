using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.TicketProvider;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class SaveTicketDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(TicketUserViewModel ticketVm)
    {
        return ticketVm.WhenAnyValue(x => x.SelectedTicket.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync( TicketUserViewModel ticketUserViewModel, ITicketVmProvider ticketVmProvider, IConnectionStateProvider connectionStateProvider )
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
            var dbState = await ticketVmProvider.CreateOrEditTicket(selectedTicket);

            ticketUserViewModel.SearchTicketDataCommand.Execute().Subscribe();
        }
        catch(DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            ticketUserViewModel.ErrorMessage = pgException.ErrorMessageFromCode(nameof(TicketUserViewModel));
        }
        catch (DbUpdateException e)
        {
            ticketUserViewModel.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            ticketUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        
        ticketUserViewModel.IsLoadingEditMode = false;
    }
    
    public SaveTicketDataCommand(TicketUserViewModel tickerUserViewModel, ITicketVmProvider ticketVmProvider, IConnectionStateProvider connectionStateProvider) : 
        base(_ => Observable.Start(async () => await SaveDataAsync(tickerUserViewModel, ticketVmProvider, connectionStateProvider)), 
        canExecute: CanExecuteCommand(tickerUserViewModel).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
    
}