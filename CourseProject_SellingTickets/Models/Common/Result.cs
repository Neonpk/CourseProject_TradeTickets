using CourseProject_SellingTickets.Interfaces.Common;

namespace CourseProject_SellingTickets.Models.Common;

public readonly record struct Result<TValue>(TValue? Value, ResultStatus Status, string? Message = null)
    : IResult<TValue>
{
    public bool IsSuccess => Status == ResultStatus.Success;

    public static Result<TValue> Success(TValue value) => new(value, ResultStatus.Success);
    public static Result<TValue> Failure(string msg) => new(default, ResultStatus.Failure, msg);
    
    // Неявное преобразование из значения в успешный результат

    public static implicit operator Result<TValue>(TValue value) => Success(value);
}
