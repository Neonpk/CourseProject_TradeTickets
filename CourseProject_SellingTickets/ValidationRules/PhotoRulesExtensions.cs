using System;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using CourseProject_SellingTickets.Models;
using ReactiveUI.Validation.Extensions;

namespace CourseProject_SellingTickets.ValidationRules;

public static class PhotoRulesExtensions
{
    public static void InitializeValidationRules(this Photo self)
    {
        self.ValidationRule(x => x.Name, x => !String.IsNullOrEmpty(x?.Trim()), "[=>] Не указано имя изображения.");
        self.ValidationRule(x => x.UrlPath, x => !String.IsNullOrEmpty(x?.Trim()), "[=>] Не указана ссылка на изображение.");
        self.ValidationRule(x => x.UrlPath, x => Regex.IsMatch(x!.Trim(), "(http[s]?:\\/\\/.*\\.(?:png|jpg|gif|svg|jpeg))"),
            "[=>] Некорректный адрес изображения.");
        
        self.ValidationContext.Changed.
            Do(_ => self.ErrorValidations = $"[Валидация]:\n--\n\n{self.ValidationContext.Text.ToSingleLine("\n\n")}" ).
            Subscribe();
    }
}