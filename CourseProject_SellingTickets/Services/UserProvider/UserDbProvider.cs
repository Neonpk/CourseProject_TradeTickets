using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.Common;
using CourseProject_SellingTickets.Interfaces.Factories;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.UserProvider;

public class UserDbProvider : IUserDbProvider
{
    private readonly ITradeTicketsDbContextFactory _dbContextFactory;
    
    public UserDbProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<User> GetUserById(Int64 id)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
                throw new DataException("[Error]: User not found.");
            
            return ToUser(user);
        }
    }
    
    public async Task<Int64> GetUserIdByLogin(string login)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            var user = await context.Users.
                    Where(x => x.Login.Equals(login)).
                    AsNoTracking(). 
                    Include(x => x.Discount).
                    Include(x => x.Photo).
                    Include(x => x.Tickets).
                    FirstOrDefaultAsync();

            if (user == null) 
                throw new DataException("[Error]: User not found.");
            
            return user.Id;
        }
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<UserDTO> userDtos = await context.Users.
                AsNoTracking().
                Include(x => x.Discount).
                Include(x => x.Photo).
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
                Include(x => x.Discount).
                Include(x => x.Photo).
                Include(x => x.Tickets).
                ToListAsync();

            return userDtos.Select(userDto => ToUser(userDto));
        }
    }

    public async Task<IResult<string>> DepositBalance(Int64 userId, decimal amount)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            try
            {
                IEnumerable<string> result = await context.Database
                    .SqlQuery<string>($"SELECT * FROM public.deposit_balance({(int)userId}, {amount})")
                    .ToListAsync();

                return Result<string>.Success(result.FirstOrDefault("The balance was not changed."));
            }
            catch (Npgsql.PostgresException ex)
            {
                return Result<string>.Failure(ex.MessageText);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(ex.Message);
            }
        }
    }

    public async Task<IResult<string>> GenerateUserAvatar(long userId, string urlPath)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            try
            {
                IEnumerable<string> result = await context.Database
                    .SqlQuery<string>($"SELECT * FROM public.generate_user_avatar({(int)userId}, {urlPath})")
                    .ToListAsync();

                return Result<string>.Success(result.FirstOrDefault("Failed to generate avatar"));
            }
            catch (Npgsql.PostgresException ex)
            {
                return Result<string>.Failure(ex.MessageText);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(ex.Message);
            }
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
            userDto.Balance,
            userDto.BirthDay,
            userDto.Passport,
            new Discount(userDto.Discount.Id, userDto.Discount.Name, userDto.Discount.DiscountSize,  userDto.Discount.Description),
            new Photo(userDto.Photo.Id, userDto.Photo.Name, userDto.Photo.UrlPath, userDto.Photo.IsDeleted)
        );
    }
    
    private UserDTO ToUserDto(User user)
    {
        var userDto = new UserDTO
        {
            Id = user.Id ?? 0,
            Login = user.Login,
            Name = user.Name,
            Role = user.Role,
            Password = user.Password,
            Balance = user.Balance,
            DiscountId = user.Discount.Id,
            BirthDay = user.BirthDay,
            Passport = user.Passport,
            PhotoId = user.Photo.Id
        };
        
        return userDto;
    }
}
