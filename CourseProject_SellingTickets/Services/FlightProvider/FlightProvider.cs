using System.Collections.Generic;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.AirlineProvider;
using CourseProject_SellingTickets.Services.PlaceProvider;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;

namespace CourseProject_SellingTickets.Services.FlightProvider;

public interface IFlightProvider
{
    public Task<IEnumerable<Flight>> GetAllFlights();
    public Task<IEnumerable<Airline>> GetAllAirlines();
    public Task<IEnumerable<Aircraft>> GetAllAircrafts();
    public Task<IEnumerable<Place>> GetAllPlaces();

    public Task<bool> CreateOrEditFlight(Flight? flight);
    public Task<bool> DeleteFlight(Flight? flight);
}
public class FlightProvider : IFlightProvider
{

    private IFlightDbProvider? _flightDbProvider;
    private IAircraftDbProvider? _aircraftDbProvider;
    private IAirlineDbProvider? _airlineDbProvider;
    private IPlaceDbProvider? _placeDbProvider;
    
    public FlightProvider( IFlightDbProvider? flightDbProvider, IAircraftDbProvider? aircraftDbProvider, 
        IAirlineDbProvider? airlineDbProvider, IPlaceDbProvider? placeDbProvider )
    {
        _flightDbProvider = flightDbProvider;
        _aircraftDbProvider = aircraftDbProvider;
        _airlineDbProvider = airlineDbProvider;
        _placeDbProvider = placeDbProvider;
    }

    public async Task<bool> CreateOrEditFlight(Flight? flight)
    {
        return await _flightDbProvider!.CreateOrEditFlight(flight);
    }

    public async Task<bool> DeleteFlight(Flight? flight)
    {
        return await _flightDbProvider!.DeleteFlight(flight);
    }
    
    public async Task<IEnumerable<Flight>> GetAllFlights()
    {
        return await _flightDbProvider!.GetAllFlights();
    }

    public async Task<IEnumerable<Airline>> GetAllAirlines()
    {
        return await _airlineDbProvider!.GetAllAirlines();
    }
    
    public async Task<IEnumerable<Aircraft>> GetAllAircrafts()
    {
        return await _aircraftDbProvider!.GetAllAircrafts();
    }
    
    public async Task<IEnumerable<Place>> GetAllPlaces()
    {
        return await _placeDbProvider!.GetAllPlaces();
    }
    
}