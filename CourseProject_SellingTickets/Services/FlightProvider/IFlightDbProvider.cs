using System.Collections.Generic;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.TradeTicketsProvider;

public interface IFlightDbProvider
{
    Task<IEnumerable<Flight>> GetAllFlights();
}