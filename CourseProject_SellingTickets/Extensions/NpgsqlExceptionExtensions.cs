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
            },
            [nameof(PlaceUserViewModel)] = new ()
            {
                [PostgresStates.UniqueViolation] = "Не удалось сохранить данные: (Место с такими данными уже существует)"
            },
            [nameof(DiscountUserViewModel)] = new ()
            {
                [PostgresStates.UniqueViolation] = "Не удалось сохранить данные: (Скидка с такими данными уже существует)"
            },
            [nameof(PhotoUserViewModel)] = new()
            {
                [PostgresStates.UniqueViolation] = "Не удалось сохранить данные: (Фото с такими данными уже существует)"
            },
            [nameof(FlightClassUserViewModel)] = new()
            {
                [PostgresStates.UniqueViolation] = "Не удалось сохранить данные: (Такой класс полета уже существует)"        
            },
            [nameof(AirlineUserViewModel)] = new()
            {
                [PostgresStates.UniqueViolation] = "Не удалось сохранить данные: (Авиалиния с таким именем уже существует)"
            },
            [nameof(RegisterUserViewModel)] = new()
            {
                [PostgresStates.UniqueViolation] = "Не удалось создать пользователя: (Такой пользователь уже существует)"
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