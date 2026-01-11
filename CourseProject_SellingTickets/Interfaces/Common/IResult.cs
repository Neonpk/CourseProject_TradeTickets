using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Interfaces.Common;

public interface IResult<out TValue>
{
    TValue? Value { get; }
    ResultStatus Status { get; }
    bool IsSuccess { get; }
    string? Message { get; }
}
