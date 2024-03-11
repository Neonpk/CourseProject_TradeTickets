using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.DiscountProvider;
using CourseProject_SellingTickets.Services.TicketProvider;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.DiscountCommands;

public class LoadDiscountDataCommand : ReactiveCommand<IEnumerable<Discount>, Task>
{
    private static async Task LoadDataAsync(DiscountUserViewModel discountUserViewModel, IDiscountVmProvider discountVmProvider, IEnumerable<Discount> filteredDiscounts)
    {
        var limitRows = discountUserViewModel.LimitRows;

        discountUserViewModel.ErrorMessage = string.Empty;
        discountUserViewModel.IsLoading = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        try
        {
            bool hasSearching = discountUserViewModel.HasSearching;

            IEnumerable<Discount> discounts =
                hasSearching ? filteredDiscounts! : await discountVmProvider.GetTopDiscounts(limitRows);

            Dispatcher.UIThread.Post(() =>
            {
                discountUserViewModel.DiscountItems.Clear();
                discountUserViewModel.DiscountItems.AddRange(discounts);
            });
            
        }
        catch (Exception e)
        {
            discountUserViewModel.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }

        discountUserViewModel.IsLoading = false;
    }
    
    public LoadDiscountDataCommand(DiscountUserViewModel discountUserViewModel, IDiscountVmProvider discountVmProvider) :
        base(filteredDiscounts => Observable.Start(async () => 
                await LoadDataAsync(discountUserViewModel, discountVmProvider, filteredDiscounts) ),
            canExecute: Observable.Return(true))
    {
    }
}