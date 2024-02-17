using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.DiscountProvider;

public interface IDiscountDbProvider
{
    Task<IEnumerable<Discount>> GetAllDiscounts();
    Task<IEnumerable<Discount>> GetTopDiscounts(int topRows = 50);

    Task<IEnumerable<Discount>> GetDiscountsByFilter(Expression<Func<DiscountDTO, bool>> searchFunc, int topRows = -1);
    
    Task<IEnumerable<Discount>> GetDiscountsByFilterSort<TKeySelector>
        ( Expression<Func<DiscountDTO, bool>> searchFunc, Expression<Func<DiscountDTO, TKeySelector>> sortFunc, SortMode? sortMode, int topRows = -1);
    
    Task<bool> CreateOrEditDiscount(Discount discount);
    Task<bool> DeleteDiscount(Discount discount);
}