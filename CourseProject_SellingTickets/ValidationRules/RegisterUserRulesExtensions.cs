using System;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

namespace CourseProject_SellingTickets.ValidationRules;

public static class RegisterUserRulesExtensions
{
    public static void InitializeValidationRules(this RegisterUserViewModel self)
    {
        IObservable<bool> passwordObservable = self.WhenAnyValue(
            x => x.Password,
            x => x.ConfirmPassword,
            (pass, confirmPass) => pass.Equals(confirmPass)
        );
        
        self.ValidationRule(x => x.Login, x => Regex.IsMatch(x!, "^[a-zA-Z]{1}[a-zA-Z0-9]{4,20}$"), "[=>] Длина логина должна составлять от 5 до 20 символов.");
        self.ValidationRule(x => x.Name, x => 
            Regex.IsMatch(x!, 
                "^(?:[А-Я]{1}[а-я]{2,15}\\s[А-Я]{1}[а-я]{1,15}\\s[А-Я]{1}[а-я]{5,15})$|^(?:[A-Z]{1}[a-z]{2,15}\\s[A-Z]{1}[a-z]{1,15}\\s[A-Z]{1}[a-z]{5,15})$"), 
            "[=>] Пример ФИО: Петров Андрей Иванович или Petrov Andrey Ivanovich.");

        self.ValidationRule(x => x.Password, x => Regex.IsMatch(x!, "[A-Za-z0-9]{5,30}"),
            "[=>] Пароль должен быть от 5 до 30 символов.");

        self.ValidationRule(x => x.ConfirmPassword, passwordObservable, "[=>] Пароли должны быть идентичны.");
        
        self.ValidationContext.Changed.
            Do(_ => self.ErrorValidations = $"[Валидация]:\n--\n\n{self.ValidationContext.Text.ToSingleLine("\n\n")}" ).
            Subscribe();
    }
}
