using System.Collections.Generic;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.PlaceProvider;

public interface IPlaceDbProvider
{
    Task<IEnumerable<Place>> GetAllPlaces();
}