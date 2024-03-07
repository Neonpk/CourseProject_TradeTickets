using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.DiscountProvider;
using CourseProject_SellingTickets.Services.TicketProvider;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.DiscountCommands;

public class SearchDiscountDataCommand : ReactiveCommand<Unit, Task<IEnumerable<Discount>?>>
{
    private static async Task<IEnumerable<Discount>> GetDiscountDataByFilter(IDiscountVmProvider discountVmProvider, string searchTerm, DiscountSearchSortModes searchMode, int limitRows = 50)
    {
         switch (searchMode)
         {
             // By Name 
             case DiscountSearchSortModes.Name:
                 return await discountVmProvider.GetDiscountsByFilter(
                     x => x.Name.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);
                
             // By Description
             case DiscountSearchSortModes.Description:
                 return await discountVmProvider.GetDiscountsByFilter(
                     x => x.Description.ToLower().StartsWith(searchTerm.ToLower()), 
                     limitRows);
             
             // By Discount size
             case DiscountSearchSortModes.DiscountSize:
                 return await discountVmProvider.GetDiscountsByFilter(
                     x => x.DiscountSize.ToString().StartsWith(searchTerm.ToLower()), 
                     limitRows);
             // Empty 
             default:
                 return new List<Discount>();
        }
    }
    private static async Task<IEnumerable<Discount>?> SearchDataAsync(DiscountUserViewModel discountUserViewModel, IDiscountVmProvider discountVmProvider)
    {
        int limitRows = discountUserViewModel.LimitRows;
        string searchTerm = discountUserViewModel.SearchTerm!;
        DiscountSearchSortModes selectedSearchMode = (DiscountSearchSortModes)discountUserViewModel.SelectedSearchMode;
        
        try
        {
            discountUserViewModel.IsLoading = true;
            IEnumerable<Discount> discounts = await GetDiscountDataByFilter(discountVmProvider, searchTerm, selectedSearchMode, limitRows);

            return discounts;
        }
        catch (Exception e)
        {
            discountUserViewModel.IsLoading = false;
            discountUserViewModel.ErrorMessage = $"Не удалось найти данные: ({e.Message})";

            return null;
        }
    }
    
    public SearchDiscountDataCommand(DiscountUserViewModel discountUserViewModel, IDiscountVmProvider discountVmProvider) : 
        base(_ => Observable.Start(async () => await SearchDataAsync(discountUserViewModel, discountVmProvider)), canExecute: Observable.Return(true))
    {
    }
}