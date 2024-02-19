using System;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using ReactiveUI.Validation.Extensions;

namespace CourseProject_SellingTickets.ValidationRules;

public static class TicketRulesExtensions
{
    public static void InitializeValidationRules(this Ticket self)
    {
        self.ValidationRule(x => x.Discount.Id, x => x.CompareTo(default) != 0, "[=>] Вид скидки не был выбрана.");
        self.ValidationRule(x => x.FlightClass.Id, x => x.CompareTo(default) != 0, "[=>] Класс полета не был выбран.");

        self.ValidationContext.Changed.
            Do(_ => self.ErrorValidations = $"[Валидация]:\n--\n\n{self.ValidationContext.Text.ToSingleLine("\n\n")}" ).
            Subscribe();
    }
}