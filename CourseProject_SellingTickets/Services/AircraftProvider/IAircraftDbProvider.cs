using System.Collections.Generic;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.AircraftProvider;

public interface IAircraftDbProvider
{
    Task<IEnumerable<Aircraft>> GetAllAircrafts();
}