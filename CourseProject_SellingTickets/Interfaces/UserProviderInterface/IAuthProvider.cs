using System;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Interfaces.UserProviderInterface;

public interface IAuthProvider
{
    Task<AuthStates> CheckUserPassword(string login, string password);
    Task<UserRoles> GetUserRole(string login);
    Task<User> GetUserById(Int64 id);
    Task<Int64> GetUserIdByLogin(string login);
    Task<int> CreateUser(User user);
}
