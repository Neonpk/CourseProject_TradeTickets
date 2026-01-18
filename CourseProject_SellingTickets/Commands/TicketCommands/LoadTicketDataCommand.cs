using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Interfaces.TicketProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class LoadTicketDataCommand : ReactiveCommand<IEnumerable<Ticket>, Task>
{
    private static async Task LoadDataAsync(TicketUserViewModel ticketUserVm, ITicketVmProvider ticketVmProvider, IEnumerable<Ticket>? filteredTickets)
    {
        try
        {
            var limitRows = ticketUserVm.LimitRows;

            ticketUserVm.ErrorMessage = string.Empty;
            ticketUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();

            if (!await isConnected)
            {
                ticketUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }
            
            bool hasSearching = ticketUserVm.HasSearching;

            IEnumerable<Ticket> tickets;
            TicketUserViewModelParam? ticketUserVmParam = ticketUserVm.TicketUserVmParam;

            if (!hasSearching)
            {
                if (ticketUserVmParam != null)
                { 
                    tickets = await ticketVmProvider.GetTicketsByFilter(
                        x => 
                            ticketUserVmParam.Include ? ticketUserVmParam.UserId.Equals(x.UserId) : x.UserId == null && !x.IsSold,
                        limitRows);
                }
                else
                {
                    tickets = await ticketVmProvider.GetTopTickets(limitRows);
                }
            }
            else
            {
                tickets = filteredTickets!;
            }

            Dispatcher.UIThread.Post(async void () =>
            {
                ticketUserVm.TicketItems.Clear();
                ticketUserVm.Discounts.Clear();
                ticketUserVm.FlightClasses.Clear();
                ticketUserVm.Flights.Clear();
                ticketUserVm.Users.Clear();
                
                ticketUserVm.TicketItems.AddRange(tickets);

                if (ticketUserVmParam != null)
                {
                    ticketUserVm.SideBarShowed = false;
                    return;
                }
                
                ticketUserVm.Discounts.AddRange(await ticketVmProvider.GetAllDiscounts());
                ticketUserVm.FlightClasses.AddRange(await ticketVmProvider.GetAllFlightClasses());
                ticketUserVm.Flights.AddRange(await ticketVmProvider.GetAllFlights());
                ticketUserVm.Users.AddRange(await ticketVmProvider.GetAllUsers());
            });

        }
        catch (Exception e)
        {
            ticketUserVm.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
        finally
        {
            ticketUserVm.IsLoading = false;
        }
    }
    
    public LoadTicketDataCommand(TicketUserViewModel ticketUserVm, ITicketVmProvider ticketProvider) :
        base(filteredTickets => Observable.Start(async () => 
                await LoadDataAsync(ticketUserVm, ticketProvider, filteredTickets) ),
            canExecute: Observable.Return(true))
    {
    }
}
