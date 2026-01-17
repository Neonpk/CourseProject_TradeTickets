using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.UserListCommands;

public class LoadUserListDataCommand : ReactiveCommand<IEnumerable<User>, Task>
{
    private static async Task LoadDataAsync(UserListViewModel userListVm, IUserListVmProvider userListVmProvider, IEnumerable<User> filteredUsers)
    {
        try
        {
            var limitRows = userListVm.LimitRows;

            userListVm.ErrorMessage = string.Empty;
            userListVm.IsLoading = true;

            ConnectionDbState.CheckConnectionState.Execute().Subscribe();
            
            bool hasSearching = userListVm.HasSearching;

            IEnumerable<User> users =
                hasSearching
                    ? filteredUsers!
                    : await userListVmProvider.GetUsersByFilter(x => x.Role.Equals("user"), limitRows);

            Dispatcher.UIThread.Post(async void() =>
            {
                userListVm.UserItems.Clear();
                userListVm.UserItems.AddRange(users);
                
                userListVm.Discounts.Clear();
                userListVm.Discounts.AddRange(await userListVmProvider.GetAllDiscounts());
            });

        }
        catch (Exception e)
        {
            userListVm.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
        finally
        {
            userListVm.IsLoading = false;   
        }
    }
    
    protected internal LoadUserListDataCommand(UserListViewModel userListVm, IUserListVmProvider userListVmProvider) : 
        base(filteredUsers => Observable.Start(async () => await LoadDataAsync(userListVm, userListVmProvider, filteredUsers)), 
            canExecute: Observable.Return(true))
    {
    }
}
