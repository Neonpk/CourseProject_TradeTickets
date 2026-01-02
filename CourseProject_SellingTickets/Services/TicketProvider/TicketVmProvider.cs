using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.DiscountProviderInterface;
using CourseProject_SellingTickets.Interfaces.FlightClassProviderInterface;
using CourseProject_SellingTickets.Interfaces.FlightProviderInterface;
using CourseProject_SellingTickets.Interfaces.TicketProviderInterface;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
namespace CourseProject_SellingTickets.Services.TicketProvider;

public class TicketVmProvider : ITicketVmProvider
{
    private ITicketDbProvider? _ticketDbProvider;
    private IDiscountDbProvider? _discountDbProvider;
    private IFlightClassDbProvider? _flightClassDbProvider;
    private IFlightDbProvider? _flightDbProvider;
    private IUserDbProvider? _userDbProvider;
    
    public TicketVmProvider( 
        ITicketDbProvider? ticketDbProvider, IDiscountDbProvider? discountDbProvider, 
        IFlightClassDbProvider? flightClassDbProvider, IFlightDbProvider? flightDbProvider,
        IUserDbProvider? userDbProvider
        )
    {
        _ticketDbProvider = ticketDbProvider;
        _discountDbProvider = discountDbProvider;
        _flightClassDbProvider = flightClassDbProvider;
        _flightDbProvider = flightDbProvider;
        _userDbProvider = userDbProvider;
    }
    
    public async Task<IEnumerable<Ticket>> GetAllTickets()
    {
        return await _ticketDbProvider!.GetAllTickets();
    }

    public async Task<IEnumerable<Ticket>> GetTopTickets(int topRows = 50)
    {
        return await _ticketDbProvider!.GetTopTickets(topRows);
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByFilter(Expression<Func<TicketDTO, bool>> searchFunc, int topRows = -1)
    {
        return await _ticketDbProvider!.GetTicketsByFilter(searchFunc, topRows);
    }

    public async  Task<IEnumerable<Ticket>> GetTicketsByFilterSort<TKeySelector>(Expression<Func<TicketDTO, bool>> searchFunc, Expression<Func<TicketDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        return await _ticketDbProvider!.GetTicketsByFilterSort(searchFunc, sortFunc, sortMode, topRows);
    }

    // Custom Providers
    
    public async Task<IEnumerable<FlightClass>> GetAllFlightClasses()
    {
        return await _flightClassDbProvider!.GetAllFlightClasses();
    }

    public async Task<IEnumerable<Discount>> GetAllDiscounts()
    {
        return await _discountDbProvider!.GetAllDiscounts();
    }

    public async Task<IEnumerable<Flight>> GetAllFlights()
    {
        return await _flightDbProvider!.GetAllFlights();
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userDbProvider!.GetUsersByFilter(x => x.Role.Equals("user"));
    }

    public async Task<int> CreateOrEditTicket(Ticket ticket)
    {
        return await _ticketDbProvider!.CreateOrEditTicket(ticket);
    }

    public async Task<int> DeleteTicket(Ticket ticket)
    {
        return await _ticketDbProvider!.DeleteTicket(ticket);
    }
}
