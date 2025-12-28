using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.UserProvider;

public interface IUserDbProvider
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<IEnumerable<User>> GetUsersByFilter(Expression<Func<UserDTO, bool>> searchFunc, int topRows = -1);
    Task<int> CreateOrEditUser(User user);
    Task<int> DeleteUser(User user);
}
