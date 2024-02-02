using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace CourseProject_SellingTickets.Converters;

public class InitializeDataErrorConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        bool hasErrorMessage = (bool)values[0]!;
        bool isConnectedToDb = (bool)values[1]!;
        
        return hasErrorMessage && !isConnectedToDb;
    }
}