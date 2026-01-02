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
    public static async Task LoginUser(AuthUserViewModel authUserViewModel, IAuthProvider authProvider)
    {
        try
        {
            ConnectionDbState.CheckConnectionState.Execute().Subscribe();
                        
            authUserViewModel.IsLoading = true;
            authUserViewModel.AuthState = await authProvider.CheckUserPassword(authUserViewModel.Login, authUserViewModel.Password);
            authUserViewModel.IsLoading = false;

            if (authUserViewModel.AuthState != AuthStates.Success) return;
                        
            switch (await authProvider.GetUserRole(authUserViewModel.Login))
            {
                case UserRoles.Admin: 
                    authUserViewModel.NavigationService!.NavigateTo<AdminUserViewModel>(); 
                    break;
                case UserRoles.Dispatcher: 
                    authUserViewModel.NavigationService!.NavigateTo<DispatcherUserViewModel>();
                    break;
                case UserRoles.User:
                    var userId = await authProvider.GetUserIdByLogin(authUserViewModel.Login);
                    authUserViewModel.NavigationService!.NavigateTo<ClientUserViewModel>(userId);
                    break;
            }

        }
        finally
        {
            authUserViewModel.IsLoading = false;
        }
    }
    
    public LoginUserCommand(AuthUserViewModel authUserViewModel, IAuthProvider authProvider) 
        : base(_ => Observable.Start(async () => await LoginUser(authUserViewModel, authProvider)), 
        canExecute: Observable.Return(true))
    {
        
    }
}
