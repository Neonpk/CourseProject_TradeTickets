using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Interfaces.DiscountProviderInterface;
using CourseProject_SellingTickets.Interfaces.FileServiceInterface;
using CourseProject_SellingTickets.Interfaces.FreeImageServiceInterface;
using CourseProject_SellingTickets.Interfaces.PhotoProviderInterface;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface.PasswordServiceInterface;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.UserProvider;

public class UserListVmProvider : IUserListVmProvider
{
    private readonly IUserDbProvider _userDbProvider;
    private readonly IDiscountDbProvider _discountDbProvider;
    private readonly IPasswordService _passwordService;
    private readonly IPhotoDbProvider _photoDbProvider;
    private readonly IFileService _fileService;
    private readonly IFreeImageService _freeImageService;
    
    public UserListVmProvider(
        IUserDbProvider userDbProvider, 
        IPhotoDbProvider photoDbProvider,
        IDiscountDbProvider discountDbProvider, 
        IPasswordService passwordService,
        IFileService fileService,
        IFreeImageService freeImageService
        )
    {
        _userDbProvider = userDbProvider;
        _discountDbProvider = discountDbProvider;
        _passwordService = passwordService;
        _photoDbProvider = photoDbProvider;
        _fileService = fileService;
        _freeImageService = freeImageService;
    }

    public string HashPassword(string password)
    {
       return _passwordService.HashPassword(password);
    }
    
    public async Task<User> GetUserById(Int64 id)
    {
        return await _userDbProvider.GetUserById(id);
    }

    public async Task<long> GetUserIdByLogin(string login)
    {
        return await _userDbProvider.GetUserIdByLogin(login);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userDbProvider.GetAllUsers();
    }

    public async Task<IEnumerable<User>> GetUsersByFilter(Expression<Func<UserDTO, bool>> searchFunc, int topRows = -1)
    {
        return await _userDbProvider.GetUsersByFilter(searchFunc, topRows);
    }

    public async Task<IResult<string>> DepositBalance(Int64 userId, decimal amount)
    {
        return await _userDbProvider.DepositBalance(userId, amount);
    }

    public async Task<IResult<string>> GenerateUserAvatar(Int64 userId, string urlPath)
    {
        return await _userDbProvider.GenerateUserAvatar(userId, urlPath);
    }

    public async Task<IResult<long>> GenerateAvatar(string urlPath)
    {
        return await _photoDbProvider.GenerateAvatar(urlPath);
    }

    public async Task<int> CreateOrEditUser(User user)
    {
        return await _userDbProvider.CreateOrEditUser(user);
    }

    public async Task<int> DeleteUser(User user)
    {
        return await _userDbProvider.DeleteUser(user);
    }

    public async Task<IEnumerable<Discount>> GetAllDiscounts()
    {
        return await _discountDbProvider.GetAllDiscounts();
    }

    public IFileService GetFileService()
    {
        return _fileService;
    }

    public IFreeImageService GetFreeImageService()
    {
        return _freeImageService;
    }
}
