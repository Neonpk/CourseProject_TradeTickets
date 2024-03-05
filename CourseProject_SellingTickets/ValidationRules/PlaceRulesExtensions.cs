using System;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using ReactiveUI.Validation.Extensions;

namespace CourseProject_SellingTickets.ValidationRules;

public static class PlaceRulesExtensions
{
    public static void InitializeValidationRules(this Place self)
    {
        self.ValidationRule(x => x.Photo.Id, x => x.CompareTo(default) != 0, "[=>] Фото не выбрано.");
        self.ValidationRule(x => x.Name, x => !String.IsNullOrEmpty(x), "[=>] Не указана страна.");
        self.ValidationRule(x => x.Description, x => !String.IsNullOrEmpty(x), "[=>] Не указано описание.");

        self.ValidationContext.Changed.
            Do(_ => self.ErrorValidations = $"[Валидация]:\n--\n\n{self.ValidationContext.Text.ToSingleLine("\n\n")}" ).
            Subscribe();
    }
}