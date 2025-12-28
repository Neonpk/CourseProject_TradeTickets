using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using DynamicData.Kernel;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.UserProvider;

public class UserDbProvider : IUserDbProvider
{
    private readonly ITradeTicketsDbContextFactory _dbContextFactory;
    
    public UserDbProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<UserDTO> userDtos = await context.Users.
                AsNoTracking().
                Include(x => x.Tickets).
                ToListAsync();
            
            return userDtos.Select(userDto => ToUser(userDto));
        }
    }

    public async Task<IEnumerable<User>> GetUsersByFilter(Expression<Func<UserDTO, bool>> searchFunc, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<UserDTO> userDtos = await context.Users.
                Where(searchFunc).
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                Include(x => x.Tickets).
                ToListAsync();

            return userDtos.Select(userDto => ToUser(userDto));
        }
    }
    
    public async Task<int> CreateOrEditUser(User user)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            UserDTO userDto = ToUserDto(user);

            if (userDto.Id.Equals(default))
                await context.Users.AddAsync(userDto);
            else
                context.Users.Attach(userDto).State = EntityState.Modified;

            return await context.SaveChangesAsync();
        }
    }

    public async Task<int> DeleteUser(User user)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            UserDTO userDto = ToUserDto(user);
            
            context.Users.Remove(userDto);
            return await context.SaveChangesAsync();
        }
    }
    
    private User ToUser(UserDTO userDto)
    {
        return new User(
            userDto.Id,
            userDto.Login,
            userDto.Name,
            userDto.Role,
            userDto.Password,
            userDto.Balance
        );
    }
    
    private UserDTO ToUserDto(User user)
    {
        return new UserDTO
        {
            Id = user.Id ?? 0,
            Login = user.Login,
            Name = user.Name,
            Role = user.Role,
            Password = user.Password,
            Balance = user.Balance
        };
    }
}
