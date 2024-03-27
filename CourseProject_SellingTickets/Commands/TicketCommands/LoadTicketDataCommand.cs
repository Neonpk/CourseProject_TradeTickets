using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.Services.TicketProvider;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class LoadTicketDataCommand : ReactiveCommand<IEnumerable<Ticket>, Task>
{
    private static async Task LoadDataAsync(TicketUserViewModel ticketUserViewModel, ITicketVmProvider ticketVmProvider, IEnumerable<Ticket> filteredTickets)
    {
        var limitRows = ticketUserViewModel.LimitRows;

        ticketUserViewModel.ErrorMessage = string.Empty;
        ticketUserViewModel.IsLoading = true;

        ConnectionDbState.CheckConnectionState.Execute()
            .Subscribe(isConnected => ticketUserViewModel.DatabaseHasConnected = isConnected.Result);
        
        try
        {
            bool hasSearching = ticketUserViewModel.HasSearching;

            IEnumerable<Ticket> tickets =
                hasSearching ? filteredTickets! : await ticketVmProvider.GetTopTickets(limitRows);

            IEnumerable<FlightClass> flightClasses = await ticketVmProvider.GetAllFlightClasses();
            IEnumerable<Discount> discounts = await ticketVmProvider.GetAllDiscounts();
            IEnumerable<Flight> flights = await ticketVmProvider.GetAllFlights();

            Dispatcher.UIThread.Post(() =>
            {
                ticketUserViewModel.Discounts.Clear();
                ticketUserViewModel.Discounts.AddRange(discounts);

                ticketUserViewModel.FlightClasses.Clear();
                ticketUserViewModel.FlightClasses.AddRange(flightClasses);

                ticketUserViewModel.Flights.Clear();
                ticketUserViewModel.Flights.AddRange(flights);
                
                ticketUserViewModel.TicketItems.Clear();
                ticketUserViewModel.TicketItems.AddRange(tickets);
            });
            
        }
        catch (Exception e)
        {
            ticketUserViewModel.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }

        ticketUserViewModel.IsLoading = false;
    }
    
    public LoadTicketDataCommand(TicketUserViewModel ticketUserViewModel, ITicketVmProvider ticketProvider) :
        base(filteredTickets => Observable.Start(async () => 
                await LoadDataAsync(ticketUserViewModel, ticketProvider, filteredTickets) ),
            canExecute: Observable.Return(true))
    {
    }
}