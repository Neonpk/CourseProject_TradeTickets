using System;
using System.Collections.Generic;
using System.Reactive.Linq;
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
    private static async Task LoadDataAsync(TicketUserViewModel ticketUserViewModel, ITicketVmProvider ticketVmProvider, IEnumerable<Ticket>? filteredTickets)
    {
        var limitRows = ticketUserViewModel.LimitRows;

        ticketUserViewModel.ErrorMessage = string.Empty;
        ticketUserViewModel.IsLoading = true;

        ConnectionDbState.CheckConnectionState.Execute()
            .Subscribe(isConnected => ticketUserViewModel.DatabaseHasConnected = isConnected.Result);

        try
        {
            bool hasSearching = ticketUserViewModel.HasSearching;

            IEnumerable<Ticket> tickets;
            TicketUserViewModelParam? ticketUserVmParam = ticketUserViewModel.TicketUserVmParam;

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
                ticketUserViewModel.TicketItems.Clear();
                ticketUserViewModel.Discounts.Clear();
                ticketUserViewModel.FlightClasses.Clear();
                ticketUserViewModel.Flights.Clear();
                ticketUserViewModel.Users.Clear();
                
                ticketUserViewModel.TicketItems.AddRange(tickets);

                if (ticketUserVmParam != null)
                {
                    ticketUserViewModel.SideBarShowed = false;
                    return;
                }
                
                ticketUserViewModel.Discounts.AddRange(await ticketVmProvider.GetAllDiscounts());
                ticketUserViewModel.FlightClasses.AddRange(await ticketVmProvider.GetAllFlightClasses());
                ticketUserViewModel.Flights.AddRange(await ticketVmProvider.GetAllFlights());
                ticketUserViewModel.Users.AddRange(await ticketVmProvider.GetAllUsers());
            });

        }
        catch (Exception e)
        {
            ticketUserViewModel.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
        finally
        {
            ticketUserViewModel.IsLoading = false;
        }
    }
    
    public LoadTicketDataCommand(TicketUserViewModel ticketUserViewModel, ITicketVmProvider ticketProvider) :
        base(filteredTickets => Observable.Start(async () => 
                await LoadDataAsync(ticketUserViewModel, ticketProvider, filteredTickets) ),
            canExecute: Observable.Return(true))
    {
    }
}
