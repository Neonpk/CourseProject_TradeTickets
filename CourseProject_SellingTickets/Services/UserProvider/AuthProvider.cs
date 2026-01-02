using System;
using System.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.UserProvider;

public class AuthProvider : IAuthProvider
{
    private readonly IUserDbProvider _userDbProvider;
    
    public AuthProvider(IUserDbProvider userDbProvider)
    {
        _userDbProvider = userDbProvider;
    }

    public async Task<User> GetUserById(Int64 id)
    {
        return await _userDbProvider.GetUserById(id);
    }

    public async Task<Int64> GetUserIdByLogin(string login)
    {
        return await _userDbProvider.GetUserIdByLogin(login);
    }

    public async Task<AuthStates> CheckUserPassword(string login, string password)
    {
        if (password.Trim() == String.Empty)
            return AuthStates.None;

        var filter = await _userDbProvider.GetUsersByFilter(x => x.Login.Equals(login), 1);

        var enumerable = filter as User[] ?? filter.ToArray();
        if (!enumerable.Any())
            return AuthStates.Failed;
        
        return enumerable.First().Password.Equals(password) ? AuthStates.Success : AuthStates.Failed;
    }

    public async Task<UserRoles> GetUserRole(string login)
    {
        var filter = await _userDbProvider.GetUsersByFilter(x => x.Login.Equals(login), 1);

        var enumerable = filter as User[] ?? filter.ToArray();
        if (!enumerable.Any())
            throw new Exception("[Error]: User not found.");

        var role = enumerable.First().Role;
        role = char.ToUpper(role.FirstOrDefault()) + role.Substring(1);
        
        return (UserRoles)Enum.Parse(typeof(UserRoles), role);
    }

    public async Task<int> CreateOrEditUser(User user)
    {
        return await _userDbProvider.CreateOrEditUser(user);
    }
}
