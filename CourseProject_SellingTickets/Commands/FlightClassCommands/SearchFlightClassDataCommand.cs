using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.FlightClassProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.FlightClassCommands;

public class SearchFlightClassDataCommand : ReactiveCommand<Unit, Task<IEnumerable<FlightClass>?>>
{
    private static async Task<IEnumerable<FlightClass>> GetFlightClassDataByFilter(IFlightClassVmProvider flightClassVmProvider, string searchTerm, FlightClassSearchSortModes searchMode, int limitRows = 50)
    {
        switch (searchMode)
        {
            // By Class name 
            case FlightClassSearchSortModes.ClassName:
                return await flightClassVmProvider.GetFlightClassesByFilter(
                    x => x.ClassName.ToLower().StartsWith(searchTerm.ToLower()), 
                    limitRows);
            // Empty 
            default:
                return new List<FlightClass>();
        }
    }
    private static async Task<IEnumerable<FlightClass>?> SearchDataAsync(FlightClassUserViewModel flightClassUserViewModel, IFlightClassVmProvider flightClassVmProvider)
    {
        int limitRows = flightClassUserViewModel.LimitRows;
        string searchTerm = flightClassUserViewModel.SearchTerm!;
        FlightClassSearchSortModes selectedSearchMode = (FlightClassSearchSortModes)flightClassUserViewModel.SelectedSearchMode;
        
        try
        {
            flightClassUserViewModel.IsLoading = true;
            IEnumerable<FlightClass> flightClasses = await GetFlightClassDataByFilter(flightClassVmProvider, searchTerm, selectedSearchMode, limitRows);

            return flightClasses;
        }
        catch (Exception e)
        {
            flightClassUserViewModel.IsLoading = false;
            flightClassUserViewModel.ErrorMessage = $"Не удалось найти данные: ({e.Message})";

            return null;
        }
    }
    
    public SearchFlightClassDataCommand(FlightClassUserViewModel flightClassUserViewModel, IFlightClassVmProvider flightClassVmProvider) : 
        base(_ => Observable.Start(async () => await SearchDataAsync(flightClassUserViewModel, flightClassVmProvider)), canExecute: Observable.Return(true))
    {
    }
}
