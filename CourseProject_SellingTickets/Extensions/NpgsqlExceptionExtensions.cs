using System;
using System.Collections.Generic;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using Npgsql;

namespace CourseProject_SellingTickets.Extensions;

public static class NpgsqlExceptionExtensions
{

    private static readonly Dictionary<Type, Dictionary<PostgresStates, string>> _vmsConstraintStates =
        new Dictionary<Type, Dictionary<PostgresStates, string>>
        {
            [typeof(FlightUserViewModel)] = new Dictionary<PostgresStates, string>
            {
                [PostgresStates.UniqueViolation] = "Не удалось сохранить данные: (Рейс с таким номером уже существует.)"
            }
        };

    // Postgres String States by Enums 
    
    private static readonly string uniqueViolation = PostgresStates.UniqueViolation.ToStringByAttributes();
    
    public static string ErrorMessageFromCode(this NpgsqlException pgException, Type viewModel)
    {
        switch (pgException.SqlState)
        {
            case var value when value!.Equals(uniqueViolation):
                
                if (!_vmsConstraintStates.ContainsKey(viewModel))
                    throw new Exception("Exception: viewModel not found");
                
                return _vmsConstraintStates[viewModel][PostgresStates.UniqueViolation];
            
            default:
                return pgException.Message;
        }
    }
    
}