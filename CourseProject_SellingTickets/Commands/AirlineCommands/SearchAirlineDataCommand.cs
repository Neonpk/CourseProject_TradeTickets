using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.AirlineProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AirlineCommands;

public class SearchAirlineDataCommand : ReactiveCommand<Unit, Task<IEnumerable<Airline>?>>
{
    private static async Task<IEnumerable<Airline>> GetAirlinesDataByFilter(IAirlineVmProvider airlineVmProvider, string searchTerm, AirlineSearchSortModes searchMode, int limitRows = 50)
    {
        switch (searchMode)
        {
            // By name 
            case AirlineSearchSortModes.Name:
                return await airlineVmProvider.GetAirlinesByFilter(
                    x => x.Name.ToLower().StartsWith(searchTerm.ToLower()), 
                    limitRows);
            // Empty 
            default:
                return new List<Airline>();
        }
    }
    private static async Task<IEnumerable<Airline>?> SearchDataAsync(AirlineUserViewModel airlineUserVm, IAirlineVmProvider airlineVmProvider)
    {
        try
        {
            int limitRows = airlineUserVm.LimitRows;
            string searchTerm = airlineUserVm.SearchTerm!;
            AirlineSearchSortModes selectedSearchMode = (AirlineSearchSortModes)airlineUserVm.SelectedSearchMode;
            
            airlineUserVm.IsLoading = true;
            IEnumerable<Airline> airlines = await GetAirlinesDataByFilter(airlineVmProvider, searchTerm, selectedSearchMode, limitRows);

            return airlines;
        }
        catch (Exception e)
        {
            airlineUserVm.IsLoading = false;
            airlineUserVm.ErrorMessage = $"Не удалось найти данные: ({e.Message})";

            return null;
        }
    }
    
    public SearchAirlineDataCommand(AirlineUserViewModel airlineUserVm, IAirlineVmProvider airlineVmProvider) : 
        base(_ => Observable.Start(async () => await SearchDataAsync(airlineUserVm, airlineVmProvider)), canExecute: Observable.Return(true))
    {
    }
}
