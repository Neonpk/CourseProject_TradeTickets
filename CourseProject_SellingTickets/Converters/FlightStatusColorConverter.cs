using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Brushes = Avalonia.Media.Brushes;

namespace CourseProject_SellingTickets.Converters;

public class FlightStatusColorConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isCanceled = (bool)values[0]!;
        DateTime arrivalTime = (DateTime)values[1]!;

        if (isCanceled)
            return Brushes.Firebrick;

        if (DateTime.Now > arrivalTime && !isCanceled)
            return Brushes.Green;
        
        return null;

    }
}