using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.UserProvider;

public interface IAuthCheckerProvider
{
    Task<AuthStates> CheckDispatcherPassword(string password);
    Task<AuthStates> CheckAdminPassword(string password);
}