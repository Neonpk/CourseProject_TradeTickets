using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;
using Exception = System.Exception;

namespace CourseProject_SellingTickets.Commands;

public class SearchFlightDataCommand : ReactiveCommand<Unit, IEnumerable<Flight>?>
{
    private static IEnumerable<Flight> GetFlightDataByFilter(IFlightVmProvider flightVmProvider, string searchTerm, FlightSearchModes searchMode, int limitRows = 50)
    {
         switch (searchMode)
         {
             // By Flight Number
             case FlightSearchModes.FlightNumber:
                 return flightVmProvider.GetFlightsByFilter(
                     x => x.FlightNumber.ToString().StartsWith(searchTerm), 
                     limitRows).Result;
                
             // By Departure Place
             case FlightSearchModes.DeparturePlace:
                 return flightVmProvider.GetFlightsByFilter(
                     x => 
                                 x.DeparturePlace!.Description.ToLower().StartsWith(searchTerm.ToLower()) 
                                 || 
                                 x.DeparturePlace.Name.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows).Result;
             
             // By Destination Place
             case FlightSearchModes.DestinationPlace:
                 return flightVmProvider.GetFlightsByFilter(
                     x => 
                         x.DestinationPlace!.Description.ToLower().StartsWith(searchTerm.ToLower()) 
                         || 
                         x.DestinationPlace.Name.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows).Result;

             // By Departure Time
             case FlightSearchModes.DepartureTime:
                 // Getting from Postgres Timestamp Format as a string
                 return flightVmProvider.GetFlightsByFilter(
                     x => 
                         TradeTicketsDbContext.DateTimeFormatToString(x.DepartureTime,"DD.MM.YYYY HH24:MI:SS").StartsWith(searchTerm), 
                     limitRows).Result;

             // By Arrival Time
             case FlightSearchModes.ArrivalTime:
                 // Getting from Postgres Timestamp Format as a string
                 return flightVmProvider.GetFlightsByFilter(
                     x => 
                         TradeTicketsDbContext.DateTimeFormatToString(x.ArrivalTime, "DD.MM.YYYY HH24:MI:SS").StartsWith(searchTerm), 
                     limitRows).Result;

             // By Aircraft Name or Aircraft Type
             case FlightSearchModes.AircraftName:
                 return flightVmProvider.GetFlightsByFilter(
                     x => 
                         x.Aircraft!.Model!.ToLower().StartsWith(searchTerm.ToLower()) 
                         || 
                         x.Aircraft.Type!.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows).Result;

             // By Total Place
             case FlightSearchModes.TotalPlace:
                 return flightVmProvider.GetFlightsByFilter(
                     x => 
                         x.TotalPlace.ToString().StartsWith(searchTerm), 
                     limitRows).Result;

             // By FreePlace
             case FlightSearchModes.FreePlace:
                 return flightVmProvider.GetFlightsByFilter(
                     x => 
                         x.FreePlace.ToString().StartsWith(searchTerm), 
                     limitRows).Result;

             // By Duration Time
             case FlightSearchModes.DurationTime:
                 return flightVmProvider.GetFlightsByFilter(
                     x => 
                         x.DurationTime.ToString().StartsWith(searchTerm), 
                     limitRows).Result;
             // Empty 
             default:
                 return new List<Flight>();
        }
    }
    
    private static IEnumerable<Flight>? SearchDataAsync(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider)
    {
        int limitRows = flightUserViewModel.LimitRows;
        string searchTerm = flightUserViewModel.SearchTerm!;
        FlightSearchModes selectedSearchMode = (FlightSearchModes)flightUserViewModel.SelectedSearchMode;
        
        try
        {
            flightUserViewModel.IsLoading = true;
            IEnumerable<Flight> flights = GetFlightDataByFilter(flightVmProvider, searchTerm, selectedSearchMode, limitRows);

            return flights;
        }
        catch (Exception e)
        {
            flightUserViewModel.IsLoading = false;
            flightUserViewModel.ErrorMessage = $"Не удалось найти данные: ({e.Message})";

            return null;
        }
    }

    public SearchFlightDataCommand(FlightUserViewModel flightUserViewModel, IFlightVmProvider flightVmProvider) : 
        base(_ => Observable.Start(() => SearchDataAsync(flightUserViewModel, flightVmProvider)), canExecute: Observable.Return(true))
    {
        
    }
}