using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace CourseProject_SellingTickets.Converters;

public class HideButtonTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (bool)(value ?? "") ? "-" : "+";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}