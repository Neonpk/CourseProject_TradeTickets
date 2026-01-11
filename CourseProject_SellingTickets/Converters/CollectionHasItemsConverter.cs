using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace CourseProject_SellingTickets.Converters;

public class CollectionHasItemsConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isNotEmpty = (bool)parameter!;
        
        int count = (int)values[0]!;
        bool hasErrorMessage = (bool)values[1]!;
        
        return (isNotEmpty ? count > 0 : count == 0) && !hasErrorMessage;
    }
}
