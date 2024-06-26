using System;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

namespace CourseProject_SellingTickets.ValidationRules;

public static class FlightRulesExtensions
{
    public static void InitializeValidationRules(this Flight self)
    {
        IObservable<bool> timeDatesObservable =
            self.WhenAnyValue(
                x => x.DepartureTime,
                x => x.ArrivalTime,
                (departureTime, arrivalTime) => departureTime.CompareTo(arrivalTime) < 0);

        IObservable<bool> placesObservable =
            self.WhenAnyValue(
                x => x.DeparturePlace.Id,
                x => x.DestinationPlace.Id,
                (departurePlaceId, destinationPlaceId) => !departurePlaceId.Equals(default) && !destinationPlaceId.Equals(default)
            && departurePlaceId.CompareTo(destinationPlaceId) != 0 );
        
        self.ValidationRule(timeDatesObservable, "[=>] Время прибытия не может быть раньше времени отправления.");
        self.ValidationRule(placesObservable, "[=>] Места не должны совпадать.");
        self.ValidationRule(x => x.Aircraft.Id, x => x.CompareTo(default) != 0, "[=>] Самолет не был выбран.");
        self.ValidationRule(x => x.Airline.Id, x => x.CompareTo(default) != 0, "[=>] Авиакомпания не была выбрана.");

        self.ValidationContext.Changed.
            Do(_ => self.ErrorValidations = $"[Валидация]:\n--\n\n{self.ValidationContext.Text.ToSingleLine("\n\n")}" ).
            Subscribe();
    }
}