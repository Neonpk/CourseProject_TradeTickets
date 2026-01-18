using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Interfaces.FlightProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;
using Exception = System.Exception;

namespace CourseProject_SellingTickets.Commands.FlightCommands;

public class SearchFlightDataCommand : ReactiveCommand<Unit, Task<IEnumerable<Flight>?>>
{
    private static async Task<IEnumerable<Flight>> GetFlightDataByFilter(IFlightVmProvider flightVmProvider, string searchTerm, FlightSearchModes searchMode, int limitRows = 50)
    {
         switch (searchMode)
         {
             // By Flight Number
             case FlightSearchModes.FlightNumber:
                 return await flightVmProvider.GetFlightsByFilter(
                     x => x.FlightNumber.ToString().StartsWith(searchTerm), 
                     limitRows);
                
             // By Departure Place
             case FlightSearchModes.DeparturePlace:
                 return await flightVmProvider.GetFlightsByFilter(
                     x => 
                                 x.DeparturePlace!.Description.ToLower().StartsWith(searchTerm.ToLower()) 
                                 || 
                                 x.DeparturePlace.Name.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);
             
             // By Destination Place
             case FlightSearchModes.DestinationPlace:
                 return await flightVmProvider.GetFlightsByFilter(
                     x => 
                         x.DestinationPlace!.Description.ToLower().StartsWith(searchTerm.ToLower()) 
                         || 
                         x.DestinationPlace.Name.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);

             // By Departure Time
             case FlightSearchModes.DepartureTime:
                 // Getting from Postgres Timestamp Format as a string
                 return await flightVmProvider.GetFlightsByFilter(
                     x => 
                         TradeTicketsDbContext.DateTimeFormatToString(x.DepartureTime,"DD.MM.YYYY HH24:MI:SS").StartsWith(searchTerm), 
                     limitRows);

             // By Arrival Time
             case FlightSearchModes.ArrivalTime:
                 // Getting from Postgres Timestamp Format as a string
                 return await flightVmProvider.GetFlightsByFilter(
                     x => 
                         TradeTicketsDbContext.DateTimeFormatToString(x.ArrivalTime, "DD.MM.YYYY HH24:MI:SS").StartsWith(searchTerm), 
                     limitRows);

             // By Aircraft Name or Aircraft Type
             case FlightSearchModes.AircraftName:
                 return await flightVmProvider.GetFlightsByFilter(
                     x => 
                         x.Aircraft!.Model.ToLower().StartsWith(searchTerm.ToLower()) 
                         || 
                         x.Aircraft.Type.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);

             // By Total Place
             case FlightSearchModes.TotalPlace:
                 return await flightVmProvider.GetFlightsByFilter(
                     x => 
                         x.TotalPlace.ToString().StartsWith(searchTerm), 
                     limitRows);

             // By FreePlace
             case FlightSearchModes.FreePlace:
                 return await flightVmProvider.GetFlightsByFilter(
                     x => 
                         x.FreePlace.ToString().StartsWith(searchTerm), 
                     limitRows);

             // By Duration Time
             case FlightSearchModes.DurationTime:
                 return await flightVmProvider.GetFlightsByFilter(
                     x => 
                         x.DurationTime.ToString().StartsWith(searchTerm), 
                     limitRows);
             
             // By Price
             case FlightSearchModes.Price:
                 return await flightVmProvider.GetFlightsByFilter(
                     x => x.Price.ToString().StartsWith(searchTerm),
                 limitRows);
             
             // Empty 
             default:
                 return new List<Flight>();
        }
    }
    
    private static async Task<IEnumerable<Flight>?> SearchDataAsync(FlightUserViewModel flightUserVm, IFlightVmProvider flightVmProvider)
    {
        try
        {
            int limitRows = flightUserVm.LimitRows;
            string searchTerm = flightUserVm.SearchTerm!;
            FlightSearchModes selectedSearchMode = (FlightSearchModes)flightUserVm.SelectedSearchMode;
            
            flightUserVm.IsLoading = true;
            IEnumerable<Flight> flights = await GetFlightDataByFilter(flightVmProvider, searchTerm, selectedSearchMode, limitRows);

            return flights;
        }
        catch (Exception e)
        {
            flightUserVm.IsLoading = false;
            flightUserVm.ErrorMessage = $"Не удалось найти данные: ({e.Message})";

            return null;
        }
    }

    public SearchFlightDataCommand(FlightUserViewModel flightUserVm, IFlightVmProvider flightVmProvider) : 
        base(_ => Observable.Start(async () => await SearchDataAsync(flightUserVm, flightVmProvider)), canExecute: Observable.Return(true))
    {
        
    }
}
