using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.AircraftProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AircraftCommands;

public class SearchAircraftDataCommand : ReactiveCommand<Unit, Task<IEnumerable<Aircraft>?>>
{
    private static async Task<IEnumerable<Aircraft>> GetAircraftDataByFilter(IAircraftVmProvider aircraftVmProvider, string searchTerm, AircraftSearchSortModes searchMode, int limitRows = 50)
    {
         switch (searchMode)
         {
             // By Model
             case AircraftSearchSortModes.Model:
                 return await aircraftVmProvider.GetAircraftsByFilter(
                     x => x.Model.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);
                
             // By Type
             case AircraftSearchSortModes.Type:
                 return await aircraftVmProvider.GetAircraftsByFilter(
                     x => x.Type.ToLower().StartsWith(searchTerm.ToLower()),
                     limitRows);
             
             // By TotalPlace
             case AircraftSearchSortModes.TotalPlace:
                 return await aircraftVmProvider.GetAircraftsByFilter(
                     x => x.TotalPlace.ToString().StartsWith(searchTerm),
                     limitRows);
             
             // Empty 
             default:
                 return new List<Aircraft>();
        }
    }
    
    private static async Task<IEnumerable<Aircraft>?> SearchDataAsync(AircraftUserViewModel aircraftUserVm, IAircraftVmProvider aircraftVmProvider)
    {
        try
        {
            int limitRows = aircraftUserVm.LimitRows;
            string searchTerm = aircraftUserVm.SearchTerm!;
            AircraftSearchSortModes selectedSearchMode = (AircraftSearchSortModes)aircraftUserVm.SelectedSearchMode;
            
            aircraftUserVm.IsLoading = true;
            IEnumerable<Aircraft> aircrafts = await GetAircraftDataByFilter(aircraftVmProvider, searchTerm, selectedSearchMode, limitRows);

            return aircrafts;
        }
        catch (Exception e)
        {
            aircraftUserVm.IsLoading = false;
            aircraftUserVm.ErrorMessage = $"Не удалось найти данные: ({e.Message})";

            return null;
        }
    }
    
    public SearchAircraftDataCommand(AircraftUserViewModel aircraftUserVm, IAircraftVmProvider aircraftVmProvider) : 
        base(_ => Observable.Start(async () => await SearchDataAsync(aircraftUserVm, aircraftVmProvider)), canExecute: Observable.Return(true))
    {
        
    }
}
