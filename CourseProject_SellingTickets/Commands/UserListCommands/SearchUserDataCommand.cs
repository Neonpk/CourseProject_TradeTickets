using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.UserListCommands;

public class SearchUserDataCommand : ReactiveCommand<Unit, Task<IEnumerable<User>>>
{
    private static async Task<IEnumerable<User>> GetUsersByFilter(IUserListVmProvider userListVmProvider, string searchTerm, UserSearchModes searchMode, int limitRows = 50)
    {
        switch (searchMode)
        {
            // By login
            case UserSearchModes.Login:
                return await userListVmProvider.GetUsersByFilter(x => 
                    x.Role.Equals("user") && x.Login.StartsWith(searchTerm.Trim()), limitRows);
            
            // By full name
            case UserSearchModes.Name:
                return await userListVmProvider.GetUsersByFilter(x => 
                    x.Role.Equals("user") && x.Name.ToLower().StartsWith(searchTerm.ToLower()), limitRows);
            
            // By Birthday
            case UserSearchModes.Birthday:
                return await userListVmProvider.GetUsersByFilter(
                    x => 
                        x.Role.Equals("user") && TradeTicketsDbContext.DateTimeFormatToString(x.BirthDay, "DD.MM.YYYY").StartsWith(searchTerm),
                    limitRows);
            
            // By Passport
            case UserSearchModes.Passport:
                return await userListVmProvider.GetUsersByFilter(x => 
                    x.Role.Equals("user") && x.Passport.StartsWith(searchTerm), limitRows);
            
            // By Discount (Description or Name or Percent)
            case UserSearchModes.Discount:
                return await userListVmProvider.GetUsersByFilter(
                    x => 
                        x.Role.Equals("user") && 
                        (x.Discount.Description.ToLower().StartsWith(searchTerm.ToLower())
                        ||
                        x.Discount.Name.ToLower().StartsWith(searchTerm.ToLower())
                        ||
                        x.Discount.DiscountSize.ToString().StartsWith(searchTerm)), 
                    limitRows);
            
            // By AvatarUrl
            case UserSearchModes.AvatarUrl:
                return await userListVmProvider.GetUsersByFilter(x =>
                    x.Role.Equals("user") && x.Photo.UrlPath.ToLower().StartsWith(searchTerm.ToLower()), limitRows);
            
            // Empty 
            default:
                return new List<User>();
        }
    }

    private static async Task<IEnumerable<User>> SearchUserData(UserListViewModel userListVm, IUserListVmProvider userListVmProvider)
    {
        try
        {
            int limitRows = userListVm.LimitRows;
            string searchTerm = userListVm.SearchTerm!;
            UserSearchModes selectedSearchMode = (UserSearchModes)userListVm.SelectedSearchMode;
            
            userListVm.IsLoading = true;
            IEnumerable<User> users = await GetUsersByFilter(userListVmProvider, searchTerm, selectedSearchMode, limitRows);

            return users;
        }
        catch (Exception e)
        {
            userListVm.ErrorMessage = $"Не удалось найти данные: ({e.Message})";
            return new List<User>();
        }
        finally
        {
            userListVm.IsLoading = false;   
        }
    }
    
    protected internal SearchUserDataCommand(UserListViewModel userListVm, IUserListVmProvider userListVmProvider) : 
        base(_ => Observable.Start(async () => await SearchUserData(userListVm, userListVmProvider)), canExecute: Observable.Return(true))
    {
    }
}
