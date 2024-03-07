using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.DiscountProvider;
using CourseProject_SellingTickets.Services.TicketProvider;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.DiscountCommands;

public class DeleteDiscountDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(DiscountUserViewModel discountUserViewModel, IDiscountVmProvider? discountProvider, IConnectionStateProvider connectionStateProvider)
    {
        discountUserViewModel.ErrorMessage = string.Empty;
        discountUserViewModel.IsLoadingEditMode = true;
        
        discountUserViewModel.DatabaseHasConnected = await connectionStateProvider.IsConnected();
        
        if (!discountUserViewModel.DatabaseHasConnected)
        {
            discountUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            discountUserViewModel.IsLoadingEditMode = false;
            return;
        }
        
        try
        {
            Discount selectedDiscount = discountUserViewModel.SelectedDiscount;
            
            var dbState = await discountProvider!.DeleteDiscount(selectedDiscount);

            discountUserViewModel.SearchDiscountDataCommand?.Execute().Subscribe();
        }
        catch (Exception e)
        {
            discountUserViewModel.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }

        discountUserViewModel.IsLoadingEditMode = false;
    }
    
    public DeleteDiscountDataCommand(DiscountUserViewModel discountUserViewModel, IDiscountVmProvider? discountProvider, IConnectionStateProvider connectionStateProvider) : 
        base(_ => Observable.Start(async () => await DeleteDataAsync(discountUserViewModel, discountProvider, connectionStateProvider)), 
            canExecute: Observable.Return(true)) 
    {
    }
}