using System;
using System.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface.PasswordServiceInterface;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.UserProvider;

public class AuthProvider : IAuthProvider
{
    private readonly IUserDbProvider _userDbProvider;
    private readonly IPasswordService _passwordService;
    
    public AuthProvider(IUserDbProvider userDbProvider, IPasswordService passwordService)
    {
        _userDbProvider = userDbProvider;
        _passwordService = passwordService;
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

        var user = enumerable.FirstOrDefault();

        if (user is null || !_passwordService.VerifyPassword(password, user.Password))
            return AuthStates.Failed;

        if (_passwordService.NeedsRehash(user.Password))
        {
            user.Password = _passwordService.HashPassword(password);
            
            if (await _userDbProvider.CreateOrEditUser(user) == 0)
                return AuthStates.Failed;
        }
        
        return AuthStates.Success;
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
        return await _userDbProvider.CreateOrEditUser(user.CloneWithPassword(
            _passwordService.HashPassword(user.Password))
        );
    }
}
