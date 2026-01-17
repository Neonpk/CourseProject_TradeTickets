using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.UserListCommands;

public class DeleteUserDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task DeleteDataAsync(UserListViewModel userListVm, IUserListVmProvider userDbProvider)
    {
        try
        {
            userListVm.ErrorMessage = string.Empty;
            userListVm.IsLoadingEditMode = true;

            var isConnected = await ConnectionDbState.CheckConnectionState.Execute().ToTask();

            if (!await isConnected)
            {
                userListVm.ErrorMessage = "Не удалось установить соединение с БД.";
                userListVm.IsLoadingEditMode = false;
                return;
            }
            
            await userDbProvider.DeleteUser(userListVm.SelectedUser);

            userListVm.SearchUserDataCommand.Execute().Subscribe();
        }
        catch (Exception e)
        {
            userListVm.ErrorMessage = $"Не удалось удалить данные: ({e.InnerException!.Message})";
        }
        finally
        {
            userListVm.IsLoadingEditMode = false;   
        }
    }

    protected internal DeleteUserDataCommand(UserListViewModel userListVm, IUserListVmProvider userListVmProvider) :
        base(_ => Observable.Start(async () => await DeleteDataAsync(userListVm, userListVmProvider)),
            canExecute: Observable.Return(true))

    {
    }
}
