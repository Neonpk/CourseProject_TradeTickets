using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Models.Common;
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

    private static async Task<IResult<Int64>> GenerateUserAvatar(IUserListVmProvider userListVmProvider, string urlPath)
    {
        var newAvatarId = await userListVmProvider.GenerateAvatar(urlPath);
                
        if (!newAvatarId.IsSuccess)
        {
            return Result<Int64>.Failure(newAvatarId.Message!);
        }
        
        return Result<Int64>.Success(newAvatarId.Value);
    }
    
    public static async Task SaveDataAsync(
        UserListViewModel userListVm, 
        IUserListVmProvider userListVmProvider
        )
    {
        try
        {
            userListVm.ErrorMessage = string.Empty;
            userListVm.IsLoading = true;
            userListVm.IsLoadingEditMode = true;

            var isConnected = await ConnectionDbState.CheckConnectionState.Execute().ToTask();
        
            if (!await isConnected)
            {
                userListVm.ErrorMessage = "Не удалось установить соединение с БД.";
                return;
            }
            
            var user = userListVm.SelectedUser;
            
            if (!string.IsNullOrEmpty(user.NewUserPassword))
            {
                user.Password = userListVmProvider.HashPassword(user.NewUserPassword);
            }
            
            if (!string.IsNullOrEmpty(user.NewPhotoUrl))
            {
                var generateUserAvatarResult = await GenerateUserAvatar(userListVmProvider, user.NewPhotoUrl);

                if (!generateUserAvatarResult.IsSuccess)
                {
                    userListVm.ErrorMessage = generateUserAvatarResult.Message!;
                    return;
                }

                user.Photo.Id = generateUserAvatarResult.Value;
            }

            if (user.Photo.SelectedFilePhoto is not null)
            {
                
                var freeImageResult = await userListVm.GenerateFreeImageCommand.Execute(
                    user.Photo.SelectedFilePhoto.GetValueOrDefault(new FileMeta())
                ).ToTask().Unwrap();

                if (!freeImageResult.IsSuccess)
                {
                    userListVm.ErrorMessage = freeImageResult.Message!;
                    return;
                }
                
                var generateUserAvatarResult = await GenerateUserAvatar(userListVmProvider, freeImageResult.Value!);

                if (!generateUserAvatarResult.IsSuccess)
                {
                    userListVm.ErrorMessage = generateUserAvatarResult.Message!;
                    return;
                }

                user.Photo.Id = generateUserAvatarResult.Value;
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
            userListVm.IsLoadingEditMode = false;
        }
    }
    
    protected internal SaveUserDataCommand(UserListViewModel userListVm, IUserListVmProvider userListVmProvider) : 
        base(_ => Observable.Start(async () =>  await SaveDataAsync(userListVm, userListVmProvider)), 
            canExecute: CanExecuteCommand(userListVm).ObserveOn(AvaloniaScheduler.Instance))
    {
    }
}
