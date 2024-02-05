using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace CourseProject_SellingTickets.Converters;

public class FlightStatusConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        string param = (string)parameter!;
        
        bool inProgress = (bool)values[0]!;
        bool isCompleted = (bool)values[1]!;
        bool isCanceled = (bool)values[2]!;

        if (isCanceled)
            return param.Equals("String") ? "Отменен" : param.Equals("Foreground") ?  Brushes.Firebrick : null;

        if (isCompleted && !isCanceled)
            return param.Equals("String") ? "Завершен" : param.Equals("Foreground") ? Brushes.DarkGreen : null;

        if (inProgress && !isCanceled)
            return param.Equals("String") ? "В процессе" : param.Equals("Foreground") ? Brushes.OrangeRed : null;

        return  param.Equals("String") ? "Нет" : param.Equals("Foreground") ? Brushes.Transparent : null;
    }
}