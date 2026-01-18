using System;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using CourseProject_SellingTickets.Models;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

namespace CourseProject_SellingTickets.ValidationRules;

public static class PhotoRulesExtensions
{
    public static void InitializeValidationRules(this Photo self)
    {
        IObservable<bool> fileObservable = self.WhenAnyValue(
            x => x.UrlPath,
            x => x.SelectedFilePhoto,
            (urlPath, selectedFilePhoto) => 
                selectedFilePhoto is not null || 
                Regex.IsMatch(urlPath.Trim(), "^https?:\\/\\/.*\\.(jpg|jpeg|png|webp|gif)$")
        );
        
        self.ValidationRule(x => x.Name, x => !String.IsNullOrEmpty(x?.Trim()), "[=>] Не указано имя изображения.");
        
        self.ValidationRule(fileObservable, "[=>] Некорректный адрес изображения или файл не выбран.");
        
        self.ValidationContext.Changed.
            Do(_ => self.ErrorValidations = $"[Валидация]:\n--\n\n{self.ValidationContext.Text.ToSingleLine("\n\n")}" ).
            Subscribe();
    }
}
