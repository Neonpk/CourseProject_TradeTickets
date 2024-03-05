using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AircraftCommands;

public class SearchAircraftDataCommand : ReactiveCommand<Unit, Task<IEnumerable<Aircraft>?>>
{
    
    private static async Task<IEnumerable<Aircraft>> GetFlightDataByFilter(IAircraftVmProvider aircraftVmProvider, string searchTerm, AircraftSearchSortModes searchMode, int limitRows = 50)
    {
         switch (searchMode)
         {
             // By Flight Number
             case AircraftSearchSortModes.Model:
                 return await aircraftVmProvider.GetAircraftsByFilter(
                     x => x.Model.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);
                
             // By Departure Place
             case AircraftSearchSortModes.Type:
                 return await aircraftVmProvider.GetAircraftsByFilter(
                     x => x.Type.ToLower().StartsWith(searchTerm.ToLower()),
                     limitRows);
             
             // By Destination Place
             case AircraftSearchSortModes.TotalPlace:
                 return await aircraftVmProvider.GetAircraftsByFilter(
                     x => x.TotalPlace.ToString().StartsWith(searchTerm),
                     limitRows);
             
             // Empty 
             default:
                 return new List<Aircraft>();
        }
    }
    
    private static async Task<IEnumerable<Aircraft>?> SearchDataAsync(AircraftUserViewModel aircraftUserViewModel, IAircraftVmProvider aircraftVmProvider)
    {
        int limitRows = aircraftUserViewModel.LimitRows;
        string searchTerm = aircraftUserViewModel.SearchTerm!;
        AircraftSearchSortModes selectedSearchMode = (AircraftSearchSortModes)aircraftUserViewModel.SelectedSearchMode;
        
        try
        {
            aircraftUserViewModel.IsLoading = true;
            IEnumerable<Aircraft> aircrafts = await GetFlightDataByFilter(aircraftVmProvider, searchTerm, selectedSearchMode, limitRows);

            return aircrafts;
        }
        catch (Exception e)
        {
            aircraftUserViewModel.IsLoading = false;
            aircraftUserViewModel.ErrorMessage = $"Не удалось найти данные: ({e.Message})";

            return null;
        }
    }
    
    public SearchAircraftDataCommand(AircraftUserViewModel aircraftUserViewModel, IAircraftVmProvider flightVmProvider) : 
        base(_ => Observable.Start(async () => await SearchDataAsync(aircraftUserViewModel, flightVmProvider)), canExecute: Observable.Return(true))
    {
        
    }
}