using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.UserClientCommands;

public class SaveUserAvatarCommand : ReactiveCommand<string, Task>
{
    private static async Task SaveAvatar(ClientBalanceUserViewModel clientBalanceUserVm, IUserDbProvider userDbProvider, string urlPath)
    {
        Int64 userId = clientBalanceUserVm.UserId;
        
        if (String.IsNullOrEmpty(urlPath))
        {
            urlPath = clientBalanceUserVm.FileUrl;
        }

        clientBalanceUserVm.IsLoading = true;
        IResult<string> result = await userDbProvider.GenerateUserAvatar(userId, urlPath);
        clientBalanceUserVm.IsLoading = false;
        
        if (!result.IsSuccess)
        {
            clientBalanceUserVm.ErrorMessage = result.Message!;
            return;
        }

        await clientBalanceUserVm.LoadUserDataCommand.Execute();
    }
    
    protected internal SaveUserAvatarCommand(ClientBalanceUserViewModel clientBalanceUserVm, IUserDbProvider userDbProvider) : 
        base(urlPath => Observable.Start(async () => await SaveAvatar(clientBalanceUserVm, userDbProvider, urlPath)), 
            Observable.Return(true))
    {
    }
}
