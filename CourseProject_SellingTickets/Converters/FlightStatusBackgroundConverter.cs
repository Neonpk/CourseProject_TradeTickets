using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Brushes = Avalonia.Media.Brushes;

namespace CourseProject_SellingTickets.Converters;

public class FlightStatusBackgroundConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        bool inProgress = (bool)values[0]!;
        bool isCompleted = (bool)values[1]!;
        bool isCanceled = (bool)values[2]!;

        if (isCanceled)
            return Brushes.Firebrick;

        if (isCompleted && !isCanceled)
            return Brushes.DarkGreen;

        if (inProgress && !isCanceled)
            return Brushes.OrangeRed;

        return Brushes.Transparent;
    }
}