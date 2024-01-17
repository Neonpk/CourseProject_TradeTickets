using System;
using System.Globalization;
using Avalonia.Data.Converters;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Converters;

public class AuthConverterStateString : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        
        #pragma warning disable
        switch ( (AuthStates)value )
        {
            case AuthStates.Success:

                return "Выполняется вход...";
                
                break;
            
            case AuthStates.Failed:

                return "Неверный пароль.";
                
                break;
        }

        return "";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}