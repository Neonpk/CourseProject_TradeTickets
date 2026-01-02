using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.DiscountProviderInterface;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.DiscountProvider;

public class DiscountDbProvider : IDiscountDbProvider
{
    private readonly ITradeTicketsDbContextFactory _dbContextFactory;
    
    public DiscountDbProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<Discount>> GetAllDiscounts()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<DiscountDTO> discountDtos = await context.Discounts.
                AsNoTracking().
                ToListAsync();

            return discountDtos.Select(discount => ToDiscount(discount));
        }
    }

    public async Task<IEnumerable<Discount>> GetTopDiscounts(int topRows = 50)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<DiscountDTO> discountDtos = await context.Discounts.
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return discountDtos.Select(discount => ToDiscount(discount));
        }
    }

    public async Task<IEnumerable<Discount>> GetDiscountsByFilter(Expression<Func<DiscountDTO, bool>> searchFunc, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<DiscountDTO> discountDtos = await context.Discounts.
                Where(searchFunc).
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return discountDtos.Select(discount => ToDiscount(discount));
        }
    }

    public async Task<IEnumerable<Discount>> GetDiscountsByFilterSort<TKeySelector>(Expression<Func<DiscountDTO, bool>> searchFunc, Expression<Func<DiscountDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<DiscountDTO> discountDtos = await context.Discounts.
                Where(searchFunc).
                OrderByModeOrDefault( sortFunc, sortMode ).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return discountDtos.Select(discount => ToDiscount(discount));
        }
    }

    public async Task<int> CreateOrEditDiscount(Discount discount)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            DiscountDTO discountDto = ToDiscountDto(discount);

            if (discountDto.Id.Equals(default))
                context.Discounts.Add(discountDto);
            else
                context.Discounts.Attach(discountDto).State = EntityState.Modified;

            return await context.SaveChangesAsync();
        }
    }

    public async Task<int> DeleteDiscount(Discount discount)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            DiscountDTO ticketDto = ToDiscountDto(discount);
            
            context.Discounts.Remove(ticketDto);
            return await context.SaveChangesAsync();
        }
    }
    
    private static Discount ToDiscount(DiscountDTO discountDto)
    {
        return new Discount( discountDto.Id, discountDto.Name, discountDto.DiscountSize, discountDto.Description );
    }
    
    private static DiscountDTO ToDiscountDto(Discount discount)
    {
        return new DiscountDTO
        {
            Id = discount.Id,
            DiscountSize = discount.DiscountSize,
            Description = discount.Description,
            Name = discount.Name
        };
    }
}
