using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.DiscountProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.DiscountCommands;

public class DeleteDiscountDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(DiscountUserViewModel discountUserVm, IDiscountVmProvider? discountProvider)
    {
        try
        {
            discountUserVm.ErrorMessage = string.Empty;
            discountUserVm.IsLoadingEditMode = true;
            discountUserVm.IsLoading = true;

            var isConnected = ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();
        
            if (!await isConnected)
            {
                discountUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }
            
            await discountProvider!.DeleteDiscount(discountUserVm.SelectedDiscount);
            discountUserVm.SearchDiscountDataCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            discountUserVm.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            discountUserVm.IsLoadingEditMode = false;
            discountUserVm.IsLoading = false;
        }
    }
    
    public DeleteDiscountDataCommand(DiscountUserViewModel discountUserVm, IDiscountVmProvider? discountProvider) : 
        base(_ => Observable.Start(async () => await DeleteDataAsync(discountUserVm, discountProvider)), 
            canExecute: Observable.Return(true)) 
    {
    }
}
