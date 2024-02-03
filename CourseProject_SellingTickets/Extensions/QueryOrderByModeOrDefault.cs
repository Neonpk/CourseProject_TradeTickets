using System;
using System.Linq;
using System.Linq.Expressions;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Extensions;

public static class QueryOrderByModeOrDefault
{
    public static IOrderedQueryable<TSource> OrderByModeOrDefault<TSource, TKeySelector>(this IQueryable<TSource> source, 
        Expression<Func<TSource, TKeySelector>> sortFunc, SortMode? sortMode)
    {

        switch (sortMode)
        {
            case SortMode.Asc:
                return source.OrderBy(sortFunc);
            
            case SortMode.Desc:
                return source.OrderByDescending(sortFunc);
            
            default:
                return source.OrderBy(sortFunc);
        }

    }
}