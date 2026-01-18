using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.UserClientCommands;

public class DepositBalanceUserCommand : ReactiveCommand<Unit, Task>
{

    public static async Task DepositBalance(ClientBalanceUserViewModel clientBalanceUserVm, IUserDbProvider userDbProvider)
    {
        var dbHasConnected =  ConnectionDbState.CheckConnectionState.Execute().ToTask().Unwrap();
        
        Int64 userId = clientBalanceUserVm.UserId;
        decimal amount = clientBalanceUserVm.Amount;
        
        if (amount <= 0 && await dbHasConnected)
        {
            clientBalanceUserVm.ResultDepositBalance = ResultStatus.Failure;
            clientBalanceUserVm.DepositBalanceMsg = "Сумма пополнения должна быть выше 0.";
            return;
        }
        
        clientBalanceUserVm.IsLoading = true;
        IResult<string> result = await userDbProvider.DepositBalance(userId, amount);
        clientBalanceUserVm.IsLoading = false;
        
        clientBalanceUserVm.ResultDepositBalance = result.Status;
        
        if (result.IsSuccess)
        {
            clientBalanceUserVm.DepositBalanceMsg = result.Value!;
            await clientBalanceUserVm.LoadUserDataCommand.Execute();
        }
        else
        {
            clientBalanceUserVm.ErrorMessage = $"Не удалось загрузить данные: {result.Message}.";
        }
    }
    
    protected internal DepositBalanceUserCommand(ClientBalanceUserViewModel clientBalanceUserVm, IUserDbProvider userDbProvider) 
        : base (_ => Observable.Start(async () => await DepositBalance(clientBalanceUserVm, userDbProvider)), Observable.Return(true) )
    {
    }
}
