using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Interfaces.UserProviderInterface;

public interface IUserDbProvider
{
    Task<User> GetUserById(Int64 id);
    Task<Int64> GetUserIdByLogin(string login);
    Task<IEnumerable<User>> GetAllUsers();
    Task<IEnumerable<User>> GetUsersByFilter(Expression<Func<UserDTO, bool>> searchFunc, int topRows = -1);
    Task<int> CreateOrEditUser(User user);
    Task<int> DeleteUser(User user);
}
