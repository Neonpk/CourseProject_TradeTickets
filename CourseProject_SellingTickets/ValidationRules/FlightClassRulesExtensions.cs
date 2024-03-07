using System;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using ReactiveUI.Validation.Extensions;

namespace CourseProject_SellingTickets.ValidationRules;

public static class FlightClassRulesExtensions
{
    public static void InitializeValidationRules(this FlightClass self)
    {
        self.ValidationRule(x => x.ClassName, x => !String.IsNullOrEmpty(x?.Trim()), "[=>] Не указан класс полета.");

        self.ValidationContext.Changed.
            Do(_ => self.ErrorValidations = $"[Валидация]:\n--\n\n{self.ValidationContext.Text.ToSingleLine("\n\n")}" ).
            Subscribe();
    }
}