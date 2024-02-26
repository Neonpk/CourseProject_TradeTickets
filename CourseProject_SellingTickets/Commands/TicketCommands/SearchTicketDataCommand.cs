using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.TicketProvider;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.TicketCommands;

public class SearchTicketDataCommand : ReactiveCommand<Unit, Task<IEnumerable<Ticket>?>>
{
    private static async Task<IEnumerable<Ticket>> GetTicketDataByFilter(ITicketVmProvider ticketVmProvider, string searchTerm, TicketSearchModes searchMode, int limitRows = 50)
    {
         switch (searchMode)
         {
             // By Ticket Number
             case TicketSearchModes.TicketNumber:
                 return await ticketVmProvider.GetTicketsByFilter(
                     x => x.Id.ToString().StartsWith(searchTerm), 
                     limitRows);
                
             // By Flight Number
             case TicketSearchModes.FlightNumber:
                 return await ticketVmProvider.GetTicketsByFilter(
                     x => x.Flight.FlightNumber.ToString().StartsWith(searchTerm), 
                     limitRows);
             
             // By Departure Place (Description or Country)
             case TicketSearchModes.DeparturePlace:
                 return await ticketVmProvider.GetTicketsByFilter(
                     x => 
                         x.Flight.DeparturePlace!.Description.ToLower().StartsWith(searchTerm.ToLower()) 
                         || 
                         x.Flight.DeparturePlace.Name.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);

             // By Destination Place ( Description or Country)
             case TicketSearchModes.DestinationPlace:
                 return await ticketVmProvider.GetTicketsByFilter(
                     x => 
                         x.Flight.DestinationPlace!.Description.ToLower().StartsWith(searchTerm.ToLower()) 
                         || 
                         x.Flight.DestinationPlace.Name.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);

             // By Flight Class
             case TicketSearchModes.FlightClass:
                 // Getting from Postgres Timestamp Format as a string
                 return await ticketVmProvider.GetTicketsByFilter(
                     x => x.FlightClass.ClassName.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);

             // By PlaceNumber
             case TicketSearchModes.PlaceNumber:
                 return await ticketVmProvider.GetTicketsByFilter(
                     x => x.PlaceNumber.ToString().StartsWith(searchTerm.ToLower()),
                     limitRows);

             // By Discount (Description or Name or Percent)
             case TicketSearchModes.DiscountType:
                 return await ticketVmProvider.GetTicketsByFilter(
                     x => 
                          x.Discount.Description.ToLower().StartsWith(searchTerm.ToLower())
                          ||
                          x.Discount.Name.ToLower().StartsWith(searchTerm.ToLower())
                          ||
                          x.Discount.DiscountSize.ToString().StartsWith(searchTerm), 
                     limitRows);

             // By Price
             case TicketSearchModes.Price:
                 return await ticketVmProvider.GetTicketsByFilter(
                     x => x.Flight.Price.ToString().StartsWith(searchTerm), 
                     limitRows);

             // By Discount Price
             case TicketSearchModes.DiscountPrice:
                 return await ticketVmProvider.GetTicketsByFilter(
                     x => (x.Flight.Price - x.Flight.Price * (x.Discount.DiscountSize * 0.01)).ToString().StartsWith(searchTerm), 
                     limitRows);
             
             // By Departure Time
             case TicketSearchModes.DepartureTime:
                 return await ticketVmProvider.GetTicketsByFilter(x => 
                     TradeTicketsDbContext.DateTimeFormatToString(x.Flight.DepartureTime,"DD.MM.YYYY HH24:MI:SS").StartsWith(searchTerm), 
                     limitRows);
             // By Arrival Time
             case TicketSearchModes.ArrivalTime:
                 return await ticketVmProvider.GetTicketsByFilter(x => 
                         TradeTicketsDbContext.DateTimeFormatToString(x.Flight.ArrivalTime,"DD.MM.YYYY HH24:MI:SS").StartsWith(searchTerm), 
                     limitRows); 
             // Empty 
             default:
                 return new List<Ticket>();
        }
    }
    private static async Task<IEnumerable<Ticket>?> SearchDataAsync(TicketUserViewModel ticketUserViewModel, ITicketVmProvider ticketVmProvider)
    {
        int limitRows = ticketUserViewModel.LimitRows;
        string searchTerm = ticketUserViewModel.SearchTerm!;
        TicketSearchModes selectedSearchMode = (TicketSearchModes)ticketUserViewModel.SelectedSearchMode;
        
        try
        {
            ticketUserViewModel.IsLoading = true;
            IEnumerable<Ticket> tickets = await GetTicketDataByFilter(ticketVmProvider, searchTerm, selectedSearchMode, limitRows);

            return tickets;
        }
        catch (Exception e)
        {
            ticketUserViewModel.IsLoading = false;
            ticketUserViewModel.ErrorMessage = $"Не удалось найти данные: ({e.Message})";

            return null;
        }
    }
    
    public SearchTicketDataCommand(TicketUserViewModel ticketUserViewModel, ITicketVmProvider ticketVmProvider) : 
        base(_ => Observable.Start(async () => await SearchDataAsync(ticketUserViewModel, ticketVmProvider)), canExecute: Observable.Return(true))
    {
    }
    
}