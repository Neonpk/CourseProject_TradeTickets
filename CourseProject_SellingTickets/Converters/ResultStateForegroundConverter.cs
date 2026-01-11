using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Converters;

public class ResultStateForegroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ResultStatus resultStatus = (ResultStatus)value!;

        switch (resultStatus)
        {
            case ResultStatus.Success:
                return Brushes.Orange;
            
            case ResultStatus.Failure:
                return Brushes.DarkRed;
        }

        return Brushes.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
