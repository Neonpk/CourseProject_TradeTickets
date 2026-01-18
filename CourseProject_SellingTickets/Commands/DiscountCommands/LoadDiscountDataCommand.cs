using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Interfaces.DiscountProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.DiscountCommands;

public class LoadDiscountDataCommand : ReactiveCommand<IEnumerable<Discount>, Task>
{
    private static async Task LoadDataAsync(DiscountUserViewModel discountUserVm, IDiscountVmProvider discountVmProvider, IEnumerable<Discount> filteredDiscounts)
    {
        try
        {
            var limitRows = discountUserVm.LimitRows;

            discountUserVm.ErrorMessage = string.Empty;
            discountUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();

            if (!await isConnected)
            {
                discountUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }

            bool hasSearching = discountUserVm.HasSearching;

            IEnumerable<Discount> discounts =
                hasSearching ? filteredDiscounts : await discountVmProvider.GetTopDiscounts(limitRows);

            Dispatcher.UIThread.Post(() =>
            {
                discountUserVm.DiscountItems.Clear();
                discountUserVm.DiscountItems.AddRange(discounts);
            });

        }
        catch (Exception e)
        {
            discountUserVm.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
        finally
        {
            discountUserVm.IsLoading = false;   
        }
    }
    
    public LoadDiscountDataCommand(DiscountUserViewModel discountUserVm, IDiscountVmProvider discountVmProvider) :
        base(filteredDiscounts => Observable.Start(async () => 
                await LoadDataAsync(discountUserVm, discountVmProvider, filteredDiscounts) ),
            canExecute: Observable.Return(true))
    {
    }
}
