using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace CourseProject_SellingTickets.Converters;

public class InvertBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return !(bool)(value ?? false);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}