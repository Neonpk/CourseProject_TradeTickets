using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.TicketProvider;

public interface ITicketVmProvider
{
    Task<IEnumerable<Ticket>> GetAllTickets();
    Task<IEnumerable<Ticket>> GetTopTickets(int topRows = 50);

    Task<IEnumerable<Ticket>> GetTicketsByFilter(Expression<Func<TicketDTO, bool>> searchFunc, int topRows = -1);
    
    Task<IEnumerable<Ticket>> GetTicketsByFilterSort<TKeySelector>
        ( Expression<Func<TicketDTO, bool>> searchFunc, Expression<Func<TicketDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1);
    
    public Task<IEnumerable<FlightClass>> GetAllFlightClasses();
    public Task<IEnumerable<Discount>> GetAllDiscounts();
    public Task<IEnumerable<Flight>> GetAllFlights();
    
    Task<bool> CreateOrEditTicket(Ticket ticket);
    Task<bool> DeleteTicket(Ticket ticket);
}