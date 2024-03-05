using System;
using System.Collections.Generic;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using Npgsql;

namespace CourseProject_SellingTickets.Extensions;

public static class NpgsqlExceptionExtensions
{

    private static readonly Dictionary<string, Dictionary<PostgresStates, string>> _vmsConstraintStates =
        new()
        {
            [nameof(FlightUserViewModel)] = new()
            {
                [PostgresStates.UniqueViolation] = "Не удалось сохранить данные: (Рейс с таким номером уже существует.)"
            },
            [nameof(TicketUserViewModel)] = new()
            {
                [PostgresStates.UniqueViolation] = "Не удалось сохранить данные: (Билет с такими данными уже существует.)"
            },
            [nameof(AircraftUserViewModel)] = new()
            {  
                [PostgresStates.UniqueViolation] = "Не удалось сохранить данные: (Самолет с такими данными уже существует)"
            }
        };

    // Postgres String States by Enums 
    
    private static readonly string _uniqueViolation = PostgresStates.UniqueViolation.ToStringByAttributes();
    
    public static string ErrorMessageFromCode(this NpgsqlException pgException, string viewModelName)
    {
        switch (pgException.SqlState)
        {
            case var value when value!.Equals(_uniqueViolation):
                
                if (!_vmsConstraintStates.ContainsKey(viewModelName))
                    throw new Exception("Exception: viewModel not found");
                
                return _vmsConstraintStates[viewModelName][PostgresStates.UniqueViolation];
            
            default:
                return pgException.Message;
        }
    }
    
}