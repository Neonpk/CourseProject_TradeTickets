using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
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
    private static IObservable<bool> CanExecuteCommand(DiscountUserViewModel discountUserVm)
    {
        return discountUserVm.WhenAnyValue(x => x.SelectedDiscount.ValidationContext.IsValid);
    }
    
    private static async Task SaveDataAsync( DiscountUserViewModel discountUserVm, IDiscountVmProvider discountVmProvider)
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
            
            await discountVmProvider.CreateOrEditDiscount(discountUserVm.SelectedDiscount);
            discountUserVm.SearchDiscountDataCommand.Execute().Subscribe();
        }
        catch (DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            discountUserVm.ErrorMessage = pgException.ErrorMessageFromCode(nameof(DiscountUserViewModel));
        }
        catch (DbUpdateException e)
        {
            discountUserVm.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            discountUserVm.ErrorMessage = $"Не удалось сохранить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            discountUserVm.IsLoadingEditMode = false;
            discountUserVm.IsLoading = false;
        }
    }
    
    public SaveDiscountDataCommand(DiscountUserViewModel discountUserVm, IDiscountVmProvider discountVmProvider) : 
        base(_ => Observable.Start(async () => await SaveDataAsync(discountUserVm, discountVmProvider)), 
        canExecute: CanExecuteCommand(discountUserVm).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
