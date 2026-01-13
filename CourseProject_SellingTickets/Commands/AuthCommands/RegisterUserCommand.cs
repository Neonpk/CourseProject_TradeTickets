using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.AuthCommands;

public class RegisterUserCommand : ReactiveCommand<Unit, Task>
{
    private static IObservable<bool> CanExecuteCommand(RegisterUserViewModel registerUserVm)
    {
        return registerUserVm.WhenAnyValue(x => x.ValidationContext.IsValid);
    }

    public static async Task RegisterUserAsync(RegisterUserViewModel registerUserVm, IAuthProvider authProvider)
    {
        registerUserVm.ErrorMessage = string.Empty;
        registerUserVm.IsLoading = true;
        
        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        
        if (!registerUserVm.DatabaseHasConnected)
        {
            registerUserVm.ErrorMessage = "Не удалось установить соединение с БД.";
            registerUserVm.IsLoading = false;
            return;
        }

        try
        {
            await authProvider.CreateOrEditUser(new User
            {
                Id = default,
                Balance = default,
                Login = registerUserVm.Login,
                Name = registerUserVm.Name,
                Password = registerUserVm.Password,
                Role = "user",
                BirthDay = registerUserVm.BirthDay!.Value.Date,
                Passport = registerUserVm.Passport
            });
            
            Thread.Sleep(1000);
            
            registerUserVm.NavigationService!.NavigateTo<AuthUserViewModel>();
        }
        catch (DbUpdateException e) when (e.InnerException is NpgsqlException pgException)
        {
            registerUserVm.ErrorMessage = pgException.ErrorMessageFromCode(nameof(RegisterUserViewModel));
        }
        catch (DbUpdateException e)
        {
            registerUserVm.ErrorMessage = e.InnerException!.Message;
        }
        catch (Exception e)
        {
            registerUserVm.ErrorMessage = $"Не удалось создать пользователя: ({e.InnerException!.Message})";
        }
        finally
        {
            registerUserVm.IsLoading = false;
        }
        
    }
    
    public RegisterUserCommand(RegisterUserViewModel registerUserViewModel, IAuthProvider authProvider) : 
        base(_ => Observable.Start(async () => await RegisterUserAsync(registerUserViewModel, authProvider)), 
            canExecute: CanExecuteCommand(registerUserViewModel).ObserveOn(AvaloniaScheduler.Instance) )
    {
    }
}
