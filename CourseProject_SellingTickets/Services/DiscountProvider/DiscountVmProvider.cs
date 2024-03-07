using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Services.DiscountProvider;

public class DiscountVmProvider : IDiscountVmProvider
{
    private readonly IDiscountDbProvider _discountDbProvider;

    public DiscountVmProvider(IDiscountDbProvider? discountVmProvider)
    {
        _discountDbProvider = discountVmProvider!;
    }
    
    public async Task<IEnumerable<Discount>> GetAllDiscounts()
    {
        return await _discountDbProvider.GetAllDiscounts();
    }

    public async Task<IEnumerable<Discount>> GetTopDiscounts(int topRows = 50)
    {
        return await _discountDbProvider.GetTopDiscounts(topRows);
    }

    public async Task<IEnumerable<Discount>> GetDiscountsByFilter(Expression<Func<DiscountDTO, bool>> searchFunc, int topRows = -1)
    {
        return await _discountDbProvider.GetDiscountsByFilter(searchFunc, topRows);
    }

    public async Task<IEnumerable<Discount>> GetDiscountsByFilterSort<TKeySelector>(Expression<Func<DiscountDTO, bool>> searchFunc, Expression<Func<DiscountDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        return await _discountDbProvider.GetDiscountsByFilterSort(searchFunc, sortFunc, sortMode, topRows);
    }

    public async Task<int> CreateOrEditDiscount(Discount discount)
    {
        return await _discountDbProvider.CreateOrEditDiscount(discount);
    }

    public async Task<int> DeleteDiscount(Discount discount)
    {
        return await _discountDbProvider.DeleteDiscount(discount);
    }
}