using System;
using System.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.UserProvider;

public class AuthCheckerProvider : IAuthCheckerProvider
{
    private readonly IUserDbProvider _userVmProvider;
    
    public AuthCheckerProvider(IUserDbProvider userVmProvider)
    {
        _userVmProvider = userVmProvider;
    }
    
    public async Task<AuthStates> CheckDispatcherPassword(string password)
    {
        if (password.Trim() == String.Empty) 
            return AuthStates.None;

        var filter = await _userVmProvider.GetUsersByFilter(x => x.Mode.Equals("dispatcher"), 1);

        if (!filter.Any())
            throw new Exception("[Error]: User not found.");
        
        return filter.First().Password.Equals(password) ? AuthStates.Success : AuthStates.Failed;
    }
    
    public async Task<AuthStates> CheckAdminPassword(string password)
    {
        if (password.Trim() == String.Empty) 
            return AuthStates.None;
        
        var filter = await _userVmProvider.GetUsersByFilter(x => x.Mode.Equals("admin"), 1);

        if (!filter.Any())
            throw new Exception("[Error]: User not found.");
        
        return filter.First().Password.Equals(password) ? AuthStates.Success : AuthStates.Failed;
    }
    
}