using System;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using ReactiveUI.Validation.Extensions;

namespace CourseProject_SellingTickets.ValidationRules;

public static class AircraftRulesExtensions
{
    public static void InitializeValidationRules(this Aircraft self)
    {
        self.ValidationRule(x => x.Photo.Id, x => x.CompareTo(default) != 0, "[=>] Фото не выбрано.");
        self.ValidationRule(x => x.Model, x => !String.IsNullOrEmpty(x?.Trim()), "[=>] Не указана модель.");
        self.ValidationRule(x => x.Type, x => !String.IsNullOrEmpty(x?.Trim()), "[=>] Не указан тип.");
        self.ValidationRule(x => x.TotalPlace, x => x > 0, "[=>] Кол-во мест не может быть меньше или равно 0.");

        self.ValidationContext.Changed.
            Do(_ => self.ErrorValidations = $"[Валидация]:\n--\n\n{self.ValidationContext.Text.ToSingleLine("\n\n")}" ).
            Subscribe();
    }
}