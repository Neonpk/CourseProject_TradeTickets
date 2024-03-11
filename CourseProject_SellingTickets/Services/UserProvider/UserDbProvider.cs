using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
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
                ToListAsync();

            return userDtos.Select(user => ToUser(user));
        }
    }

    public async Task<IEnumerable<User>> GetUsersByFilter(Expression<Func<UserDTO, bool>> searchFunc, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<UserDTO> ticketDtos = await context.Users.
                Where(searchFunc).
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return ticketDtos.Select(ticket => ToUser(ticket));
        }
    }
    
    private User ToUser(UserDTO userDto)
    {
        return new User(userDto.Id, userDto.Mode, userDto.Password);
    }
}