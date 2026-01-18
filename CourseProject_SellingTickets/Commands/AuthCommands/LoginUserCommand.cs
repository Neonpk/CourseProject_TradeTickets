using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AuthCommands;

public class LoginUserCommand : ReactiveCommand<Unit, Task>
{
    public static async Task LoginUser(AuthUserViewModel authUserVm, IAuthProvider authProvider)
    {
        try
        {
            ConnectionDbState.CheckConnectionState.Execute().Subscribe();
                        
            authUserVm.IsLoading = true;
            authUserVm.AuthState = await authProvider.CheckUserPassword(authUserVm.Login, authUserVm.Password);
            authUserVm.IsLoading = false;

            if (authUserVm.AuthState != AuthStates.Success) return;
                        
            switch (await authProvider.GetUserRole(authUserVm.Login))
            {
                case UserRoles.Admin: 
                    authUserVm.NavigationService!.NavigateTo<AdminUserViewModel>(); 
                    break;
                case UserRoles.Dispatcher: 
                    authUserVm.NavigationService!.NavigateTo<DispatcherUserViewModel>();
                    break;
                case UserRoles.User:
                    var userId = await authProvider.GetUserIdByLogin(authUserVm.Login);
                    authUserVm.NavigationService!.NavigateTo<ClientUserViewModel>(userId);
                    break;
            }

        }
        finally
        {
            authUserVm.IsLoading = false;
        }
    }
    
    public LoginUserCommand(AuthUserViewModel authUserVm, IAuthProvider authProvider) 
        : base(_ => Observable.Start(async () => await LoginUser(authUserVm, authProvider)), 
        canExecute: Observable.Return(true))
    {
        
    }
}
