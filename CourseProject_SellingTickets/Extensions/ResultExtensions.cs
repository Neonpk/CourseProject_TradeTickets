using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.CommonInterface;

namespace CourseProject_SellingTickets.Extensions;

public static class ResultExtensions
{
    public static async Task<TValue?> GetValueOrNull<TValue>(this Task<IResult<TValue>> task) 
        where TValue : struct
    {
        var result = await task;
        return result.IsSuccess ? result.Value : null;
    }
}
