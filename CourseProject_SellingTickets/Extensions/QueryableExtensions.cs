using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Extensions;

public static class QueryableExtensions
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
    public static IQueryable<TSource> TakeOrDefault<TSource>(this IQueryable<TSource> source, int count)
    {
        return count.Equals(-1) ? source : source.Take(count);
    }
    
}