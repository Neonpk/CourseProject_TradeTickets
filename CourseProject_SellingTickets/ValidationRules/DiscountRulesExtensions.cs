using System;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using ReactiveUI.Validation.Extensions;

namespace CourseProject_SellingTickets.ValidationRules;

public static class DiscountRulesExtensions
{
    public static void InitializeValidationRules(this Discount self)
    {
        self.ValidationRule(x => x.Name, x => !String.IsNullOrEmpty(x?.Trim()), "[=>] Не указано имя наименование скидки.");
        self.ValidationRule(x => x.Description, x => !String.IsNullOrEmpty(x?.Trim()), "[=>] Не указано описание скидки.");
        self.ValidationRule(x => x.DiscountSize, x => x is >= 0 and <= 100, "[=>] Размер скидки задается в процентах от 0 до 100.");

        self.ValidationContext.Changed.
            Do(_ => self.ErrorValidations = $"[Валидация]:\n--\n\n{self.ValidationContext.Text.ToSingleLine("\n\n")}" ).
            Subscribe();
    }
}