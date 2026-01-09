using System;
using System.Globalization;
using Avalonia.Data.Converters;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Converters;

public class HideTicketFlightActionColumnConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TicketUserViewModelParam param && param.Include) return false;

        return true;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
