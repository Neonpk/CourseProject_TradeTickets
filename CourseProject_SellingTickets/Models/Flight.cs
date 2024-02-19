using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CourseProject_SellingTickets.Helpers;
using CourseProject_SellingTickets.ValidationRules;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Helpers;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Flight : ReactiveObject, IValidatableViewModel
{
    // Main Model 

    private System.Int64 _id;
    public System.Int64 Id { get => _id; set { _id = value; this.RaiseAndSetIfChanged(ref _id, value); } }
    
    private System.Int64 _flightNumber;
    public System.Int64 FlightNumber { get => _flightNumber; set => this.RaiseAndSetIfChanged(ref _flightNumber, value); }

    private Place _departurePlace;
    public Place DeparturePlace { get => _departurePlace; set => this.RaiseAndSetIfChanged(ref _departurePlace, value); }

    private DateTime _departureTime;
    public DateTime DepartureTime { get => _departureTime; set => this.RaiseAndSetIfChanged(ref _departureTime, value); }

    private Place _destinationPlace;
    public Place DestinationPlace { get => _destinationPlace; set => this.RaiseAndSetIfChanged(ref _destinationPlace, value); }

    private DateTime _arrivalTime;
    public DateTime ArrivalTime { get => _arrivalTime; set => this.RaiseAndSetIfChanged(ref _arrivalTime, value); }

    private Aircraft _aircraft;
    public Aircraft Aircraft { get => _aircraft; set => this.RaiseAndSetIfChanged(ref _aircraft, value); }

    private Airline _airline;
    public Airline Airline { get => _airline; set => this.RaiseAndSetIfChanged(ref _airline, value); }

    private bool _isCanceled;
    public bool IsCanceled { get => _isCanceled; set => this.RaiseAndSetIfChanged(ref _isCanceled, value); }

    // Non Observable
    public int TotalPlace { get; }
    public int FreePlace { get; }
    public System.TimeSpan DurationTime { get; }
    
    // Custom Properties

    private Task<Bitmap?>? _departurePlaceImage;
    public Task<Bitmap?> DeparturePlaceImage { get => _departurePlaceImage ??= ImageHelper.LoadFromWeb(new Uri(DeparturePlace.Photo.UrlPath)); }

    private Task<Bitmap?>? _destinationPlaceImage;
    public Task<Bitmap?> DestinationPlaceImage { get => _destinationPlaceImage ??= ImageHelper.LoadFromWeb(new Uri(DestinationPlace.Photo.UrlPath)); }

    private Task<Bitmap?>? _aircraftImage;
    public Task<Bitmap?> AircraftImage { get => _aircraftImage ??= ImageHelper.LoadFromWeb(new Uri(Aircraft.Photo.UrlPath)); }

    // Flight Status
    public bool IsCompleted { get => DateTime.Now > ArrivalTime && !IsCanceled; }
    public bool InProgress { get => DateTime.Now > DepartureTime && DateTime.Now < ArrivalTime && !IsCanceled; }

    // Validations 
    
    private string _errorValidations;
    public string ErrorValidations { get => _errorValidations; set => this.RaiseAndSetIfChanged(ref _errorValidations, value); }
    public ValidationContext ValidationContext { get; } = new ValidationContext();
    
    //Constructor
    
    public Flight()
    {
        DeparturePlace = new Place();
        DestinationPlace = new Place();
        Aircraft = new Aircraft();
        Airline = new Airline();

        // Validations 
        this.InitializeValidationRules();
    }
    
    public Flight(FlightDTO flightDto)
    {
    // Privates 

    // Public Actions

    // Main Model

        Id = flightDto.Id;
        
        FlightNumber = flightDto.FlightNumber;
        
        // reference
        DeparturePlace = new Place(
            flightDto.DeparturePlaceId,
            flightDto.DeparturePlace.Name, 
            flightDto.DeparturePlace.Description, 
            new Photo(flightDto.DeparturePlace.Photo.Id, flightDto.DeparturePlace.Photo.Name, flightDto.DeparturePlace.Photo.UrlPath, flightDto.DeparturePlace.Photo.IsDeleted)
        );

        DepartureTime = flightDto.DepartureTime;
        
        //reference
        DestinationPlace = new Place(
            flightDto.DestinationPlaceId,
            flightDto.DestinationPlace.Name, 
            flightDto.DestinationPlace.Description, 
            new Photo(flightDto.DestinationPlace.Photo.Id, flightDto.DestinationPlace.Photo.Name, flightDto.DestinationPlace.Photo.UrlPath, flightDto.DestinationPlace.Photo.IsDeleted)
        );

        ArrivalTime = flightDto.ArrivalTime;

        // reference
        Aircraft = new Aircraft( 
            flightDto.AircraftId, 
            flightDto.Aircraft.Model,
            flightDto.Aircraft.Type,
            flightDto.Aircraft.TotalPlace,
            new Photo(flightDto.Aircraft.Photo.Id, flightDto.Aircraft.Photo.Name, flightDto.Aircraft.Photo.UrlPath, flightDto.Aircraft.Photo.IsDeleted )
        );
        
        // reference 
        Airline = new Airline(
            flightDto.AirlineId,
            flightDto.Airline.Name
        );
        
        TotalPlace = flightDto.TotalPlace;
        FreePlace = flightDto.FreePlace;
        DurationTime = flightDto.DurationTime;
        IsCanceled = flightDto.IsCanceled;
        
        // Validations 
        this.InitializeValidationRules();
    }
    
    
    public override bool Equals(object? obj)
    {
        if (obj is Flight o)
        {
            return Id.Equals(o.Id) &&
                   FlightNumber.Equals(o.FlightNumber) &&
                   DeparturePlace.Equals(o.DeparturePlace) &&
                   DepartureTime.Equals(o.DepartureTime) &&
                   DestinationPlace.Equals(o.DestinationPlace) &&
                   ArrivalTime.Equals(o.ArrivalTime) &&
                   Aircraft.Equals(o.Aircraft) &&
                   TotalPlace.Equals(o.TotalPlace) &&
                   FreePlace.Equals(o.FreePlace) &&
                   DurationTime.Equals(o.DurationTime) &&
                   Airline.Equals(o.Airline) &&
                   IsCanceled.Equals(o.IsCanceled);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}