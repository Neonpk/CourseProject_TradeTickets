using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.UserProvider;

public interface IAuthProvider
{
    Task<AuthStates> CheckUserPassword(string login, string password);
    Task<UserRoles> GetUserRole(string login);
    Task<int> CreateOrEditUser(User user);
}
