using System;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using CourseProject_SellingTickets.Models;
using ReactiveUI.Validation.Extensions;

namespace CourseProject_SellingTickets.ValidationRules;

public static class UserRulesExtensions
{
    public static void InitializeValidationRules(this User self)
    {
        
        self.ValidationRule(x => x.Login, x => Regex.IsMatch(x!, "^[a-zA-Z]{1}[a-zA-Z0-9]{4,20}$"), "[=>] Длина логина должна составлять от 5 до 20 символов.");
        self.ValidationRule(x => x.Name, x => 
                Regex.IsMatch(x!, 
                    "^(?:[А-Я]{1}[а-я]{2,15}\\s[А-Я]{1}[а-я]{1,15}\\s[А-Я]{1}[а-я]{5,15})$|^(?:[A-Z]{1}[a-z]{2,15}\\s[A-Z]{1}[a-z]{1,15}\\s[A-Z]{1}[a-z]{5,15})$"), 
            "[=>] Пример ФИО: Петров Андрей Иванович или Petrov Andrey Ivanovich.");

        self.ValidationRule(x => x.BirthDay, x => x.Date < DateTime.Today, 
            "[=>] Дата рождения не указана.");
        
        self.ValidationRule(x => x.Passport, x => Regex.IsMatch(x!, "^\\d{10}$"),
            "[=>] Паспортные данные должны содержать 10 цифр без пробелов");
        
        self.ValidationRule(x => x.NewUserPassword, x => Regex.IsMatch(x!, "^[A-Za-z0-9]{5,30}$|^$"),
            "[=>] Пароль должен быть от 5 до 30 символов.");
        
        self.ValidationRule(x => x.NewPhotoUrl, x => Regex.IsMatch(x!.Trim(), "^https?:\\/\\/.*\\.(jpg|jpeg|png|webp|gif)$|^$"),
            "[=>] Некорректный адрес изображения.");
        
        self.ValidationRule(x => x.Discount.Id, x => x.CompareTo(default) != 0, "[=>] Вид скидки не был выбран.");
        
        self.ValidationContext.Changed.
            Do(_ => self.ErrorValidations = $"[Валидация]:\n--\n\n{self.ValidationContext.Text.ToSingleLine("\n\n")}" ).
            Subscribe();
    }
}
