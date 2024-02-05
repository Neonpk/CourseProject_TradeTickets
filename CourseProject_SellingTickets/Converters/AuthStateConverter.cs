using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Converters;

public class AuthStateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string param = (string)parameter!;
        
        #pragma warning disable
        switch ( (AuthStates)value )
        {
            case AuthStates.Success:
                return param.Equals("String") ? "Выполняется вход..." : param.Equals("Foreground") ? Brushes.Orange : null;
            
            case AuthStates.Failed:
                return param.Equals("String") ? "Неверный пароль." : param.Equals("Foreground") ? Brushes.Red : null;
        }

        return param.Equals("String") ? "" : param.Equals("Foreground") ? Brushes.Transparent : null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}