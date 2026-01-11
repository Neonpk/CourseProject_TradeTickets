using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.UserClientCommands;

public class LoadUserDataCommand : ReactiveCommand<Unit, Task>
{
    private static async Task GetUserData(ClientBalanceUserViewModel clientBalanceUserVm, IUserDbProvider userDbProvider)
    {
        try
        {
            ConnectionDbState.CheckConnectionState.Execute().Subscribe();

            clientBalanceUserVm.ErrorMessage = String.Empty;
            clientBalanceUserVm.IsLoading = true;

            User user = await userDbProvider.GetUserById(clientBalanceUserVm.UserId);

            clientBalanceUserVm.UserName = user.Name;
            clientBalanceUserVm.Balance = user.Balance;
        }
        catch (Exception e)
        {
            clientBalanceUserVm.ErrorMessage = $"Не удалось загрузить данные: ({e.Message})";
        }
        finally
        {
            clientBalanceUserVm.IsLoading = false;
        }
    }
    
    protected internal LoadUserDataCommand(ClientBalanceUserViewModel clientBalanceUserVm, IUserDbProvider userDbProvider) 
        : base (_ => Observable.Start(async () => await GetUserData(clientBalanceUserVm, userDbProvider)), Observable.Return(true) )
    {
    }
}
