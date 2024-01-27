using System.Collections.Generic;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.AirlineProvider;

public interface IAirlineDbProvider
{
    Task<IEnumerable<Airline>> GetAllAirlines();
}