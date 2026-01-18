using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Interfaces.FileServiceInterface;
using CourseProject_SellingTickets.Interfaces.FreeImageServiceInterface;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Interfaces.UserProviderInterface;

public interface IUserListVmProvider
{
    Task<User> GetUserById(Int64 id);
    
    Task<Int64> GetUserIdByLogin(string login);
    
    Task<IEnumerable<User>> GetAllUsers();
    
    Task<IEnumerable<User>> GetUsersByFilter(Expression<Func<UserDTO, bool>> searchFunc, int topRows = -1);
    
    Task<IResult<string>> DepositBalance(Int64 userId, decimal amount);

    Task<IResult<string>> GenerateUserAvatar(Int64 userId, string urlPath);
    
    Task<IResult<Int64>> GenerateAvatar(string urlPath);
    
    Task<int> CreateOrEditUser(User user);
    
    Task<int> DeleteUser(User user);
    
    // Custom 
    
    IFileService GetFileService();
    
    IFreeImageService GetFreeImageService();
    
    Task<IEnumerable<Discount>> GetAllDiscounts();
    
    string HashPassword(string password);
}
