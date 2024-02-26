using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace CourseProject_SellingTickets.Converters;

public class FlightStatusConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        string param = parameter != null ? (string)parameter : String.Empty;

        foreach(var item in values)
        {
            Console.WriteLine("{0} = {1}", item, item is UnsetValueType);
        }
        
        if (values.Any(x => x is UnsetValueType))
            return param.Equals("String") ? "" : param.Equals("Foreground") ? Brushes.Transparent : false;

        bool isCanceled = (bool)values[0]!;
        bool inProgress = (bool)values[1]!;
        bool isCompleted = (bool)values[2]!;

        if (isCanceled)
            return param.Equals("String") ? "Отменен" : param.Equals("Foreground") ?  Brushes.Firebrick : true;

        if (isCompleted && !isCanceled)
            return param.Equals("String") ? "Завершен" : param.Equals("Foreground") ? Brushes.DarkGreen : true;

        if (inProgress && !isCanceled)
            return param.Equals("String") ? "В процессе" : param.Equals("Foreground") ? Brushes.OrangeRed : true;

        return param.Equals("String") ? "Нет" : param.Equals("Foreground") ? Brushes.Transparent : false;
    }
}