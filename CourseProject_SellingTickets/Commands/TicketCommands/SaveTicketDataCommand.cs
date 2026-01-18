using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.TicketProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class SaveTicketDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(TicketUserViewModel ticketUserVm)
    {
        return ticketUserVm.WhenAnyValue(x => x.SelectedTicket.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync(TicketUserViewModel ticketUserVm, ITicketVmProvider ticketVmProvider)
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
                ticketUserVm.IsLoadingEditMode = false;
                return;
            }

            await ticketVmProvider.CreateOrEditTicket(ticketUserVm.SelectedTicket);
            ticketUserVm.SearchTicketDataCommand.Execute().Subscribe();
        }
        catch (DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            ticketUserVm.ErrorMessage = pgException.ErrorMessageFromCode(nameof(TicketUserViewModel));
        }
        catch (DbUpdateException e)
        {
            ticketUserVm.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            ticketUserVm.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            ticketUserVm.IsLoadingEditMode = false;
            ticketUserVm.IsLoading = false;
        }
    }
    
    public SaveTicketDataCommand(TicketUserViewModel ticketUserVm, ITicketVmProvider ticketVmProvider) : 
        base(_ => Observable.Start(async () => await SaveDataAsync(ticketUserVm, ticketVmProvider)), 
        canExecute: CanExecuteCommand(ticketUserVm).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
