using System;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using ReactiveUI.Validation.Extensions;

namespace CourseProject_SellingTickets.ValidationRules;

public static class AirlineRulesExtensions
{
    public static void InitializeValidationRules(this Airline self)
    {
        self.ValidationRule(x => x.Name, x => !String.IsNullOrEmpty(x?.Trim()), "[=>] Не указано имя авиалинии.");

        self.ValidationContext.Changed.
            Do(_ => self.ErrorValidations = $"[Валидация]:\n--\n\n{self.ValidationContext.Text.ToSingleLine("\n\n")}" ).
            Subscribe();
    }
}