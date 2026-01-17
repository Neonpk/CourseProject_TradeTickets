using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.UserListCommands;

public class SaveUserDataCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(UserListViewModel userListVm)
    {
        return userListVm.WhenAnyValue(x => x.SelectedUser.ValidationContext.IsValid);
    }

    public static async Task SaveDataAsync(UserListViewModel userListVm, IUserListVmProvider userListVmProvider)
    {
        userListVm.ErrorMessage = string.Empty;
        userListVm.IsLoading = true;

        var isConnected = await ConnectionDbState.CheckConnectionState.Execute().ToTask();
        
        if (!await isConnected)
        {
            userListVm.ErrorMessage = "Не удалось установить соединение с БД.";
            userListVm.IsLoading = false;
            return;
        }

        try
        {
            var user = userListVm.SelectedUser;
            
            if (!string.IsNullOrEmpty(user.NewUserPassword))
            {
                user = user.CloneWithPassword(
                    userListVmProvider.HashPassword(user.NewUserPassword)
                );
            }
            
            await userListVmProvider.CreateOrEditUser(user);
            userListVm.SearchUserDataCommand.Execute().Subscribe();
        }
        catch (DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            userListVm.ErrorMessage = pgException.ErrorMessageFromCode(nameof(RegisterUserViewModel));
        }
        catch (DbUpdateException e)
        {
            userListVm.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            userListVm.ErrorMessage = $"Не удалось создать пользователя: ({e.InnerException!.Message})";
        }
        finally
        {
            userListVm.IsLoading = false;
        }
        
    }
    
    protected internal SaveUserDataCommand(UserListViewModel userListVm, IUserListVmProvider userListVmProvider) : 
        base(_ => Observable.Start(async () =>  await SaveDataAsync(userListVm, userListVmProvider)), 
            canExecute: CanExecuteCommand(userListVm).ObserveOn(AvaloniaScheduler.Instance))
    {
    }
}
