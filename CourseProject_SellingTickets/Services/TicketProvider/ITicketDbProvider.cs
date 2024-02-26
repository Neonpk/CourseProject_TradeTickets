using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.TicketProvider;

public interface ITicketDbProvider
{
    Task<IEnumerable<Ticket>> GetAllTickets();
    Task<IEnumerable<Ticket>> GetTopTickets(int topRows = 50);

    Task<IEnumerable<Ticket>> GetTicketsByFilter(Expression<Func<TicketDTO, bool>> searchFunc, int topRows = -1);
    
    Task<IEnumerable<Ticket>> GetTicketsByFilterSort<TKeySelector>
        ( Expression<Func<TicketDTO, bool>> searchFunc, Expression<Func<TicketDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1);
    
    Task<int> CreateOrEditTicket(Ticket ticket);
    Task<int> DeleteTicket(Ticket ticket);
}