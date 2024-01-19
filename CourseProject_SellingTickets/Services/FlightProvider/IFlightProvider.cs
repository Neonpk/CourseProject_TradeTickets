using System.Collections.Generic;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.TradeTicketsProvider;

public interface IFlightProvider
{
    Task<IEnumerable<Flight>> GetAllFlights();
}