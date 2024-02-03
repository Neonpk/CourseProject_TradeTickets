using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace CourseProject_SellingTickets.Converters;

public class CompletedFlightStringConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isCompleted = (bool)values[0]!;
        bool inProgress = (bool)values[1]!;

        return isCompleted ? "Да" : inProgress ? "В процессе" : "Нет";
    }
}