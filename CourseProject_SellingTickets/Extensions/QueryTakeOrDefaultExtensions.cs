using System.Linq;

namespace CourseProject_SellingTickets.Extensions;

public static class QueryTakeOrDefaultExtensions
{
    public static IQueryable<TSource> TakeOrDefault<TSource>(this IQueryable<TSource> source, int count)
    {
        return count.Equals(-1) ? source : source.Take(count);
    }
}