using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Converters;

public class AuthConverterStateColor : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        #pragma warning disable
        switch ( (AuthStates)value )
        {
            case AuthStates.Success:
                
                return Brushes.Orange;
                
                break;
            
            case AuthStates.Failed:

                return Brushes.Red;
                
                break;
        }

        return Brushes.White;

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}