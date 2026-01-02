using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.DiscountProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.DiscountCommands;

public class SaveDiscountDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(DiscountUserViewModel discountVm)
    {
        return discountVm.WhenAnyValue(x => x.SelectedDiscount.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync( DiscountUserViewModel discountUserViewModel, IDiscountVmProvider discountVmProvider)
    {
        discountUserViewModel.ErrorMessage = string.Empty;
        discountUserViewModel.IsLoadingEditMode = true;

        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        if (!discountUserViewModel.DatabaseHasConnected)
        {
            discountUserViewModel.ErrorMessage = "Не удалось установить соединение с БД.";
            discountUserViewModel.IsLoadingEditMode = false;
            return;
        }

        try
        {
            Discount selectedDiscount = discountUserViewModel.SelectedDiscount;
            var dbState = await discountVmProvider.CreateOrEditDiscount(selectedDiscount);

            discountUserViewModel.SearchDiscountDataCommand.Execute().Subscribe();
        }
        catch(DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            discountUserViewModel.ErrorMessage = pgException.ErrorMessageFromCode(nameof(DiscountUserViewModel));
        }
        catch (DbUpdateException e)
        {
            discountUserViewModel.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            discountUserViewModel.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        
        discountUserViewModel.IsLoadingEditMode = false;
    }
    
    public SaveDiscountDataCommand(DiscountUserViewModel discountUserViewModel, IDiscountVmProvider discountVmProvider) : 
        base(_ => Observable.Start(async () => await SaveDataAsync(discountUserViewModel, discountVmProvider)), 
        canExecute: CanExecuteCommand(discountUserViewModel).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
