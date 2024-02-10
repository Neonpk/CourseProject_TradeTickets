using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CourseProject_SellingTickets.Helpers;
using CourseProject_SellingTickets.ViewModels;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Flight : ObservableObject
{
    // Main Model 

    private System.Int64? _id;
    public System.Int64? Id { get => _id; set { _id = value; OnPropertyChanged(nameof(Id)); } }
    
    private System.Int64 _flightNumber;
    public System.Int64 FlightNumber { get => _flightNumber; set { _flightNumber = value; OnPropertyChanged(nameof(FlightNumber)); } }

    private Place _departurePlace;
    public Place DeparturePlace { get => _departurePlace; set { _departurePlace = value; OnPropertyChanged(nameof(DeparturePlace)); } }

    private DateTime _departureTime;
    public DateTime DepartureTime { get => _departureTime; set { _departureTime = value; OnPropertyChanged(nameof(DepartureTime)); } }

    private Place _destinationPlace;
    public Place DestinationPlace { get => _destinationPlace; set { _destinationPlace = value; OnPropertyChanged(nameof(DestinationPlace)); } }

    private DateTime _arrivalTime;
    public DateTime ArrivalTime { get => _arrivalTime; set { _arrivalTime = value; OnPropertyChanged(nameof(ArrivalTime)); } }

    private Aircraft _aircraft;
    public Aircraft Aircraft { get => _aircraft; set { _aircraft = value; OnPropertyChanged(nameof(Aircraft)); } }

    private Airline _airline;
    public Airline Airline { get => _airline; set { _airline = value; OnPropertyChanged(nameof(Airline)); } }

    private bool _isCanceled;
    public bool IsCanceled { get => _isCanceled; set { _isCanceled = value; OnPropertyChanged(nameof(IsCanceled)); } }

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

    //Constructor

    public Flight()
    {
        Id = null;
        DeparturePlace = new Place();
        DestinationPlace = new Place();
        Aircraft = new Aircraft();
        Airline = new Airline();
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
            new Photo(flightDto.DeparturePlace.Photo.Name, flightDto.DeparturePlace.Photo.UrlPath, flightDto.DeparturePlace.Photo.IsDeleted)
        );

        DepartureTime = flightDto.DepartureTime;
        
        //reference
        DestinationPlace = new Place(
            flightDto.DestinationPlaceId,
            flightDto.DestinationPlace.Name, 
            flightDto.DestinationPlace.Description, 
            new Photo(flightDto.DestinationPlace.Photo.Name, flightDto.DestinationPlace.Photo.UrlPath, flightDto.DestinationPlace.Photo.IsDeleted)
        );

        ArrivalTime = flightDto.ArrivalTime;

        // reference
        Aircraft = new Aircraft( 
            flightDto.AircraftId, 
            flightDto.Aircraft.Model,
            flightDto.Aircraft.Type,
            flightDto.Aircraft.TotalPlace,
            new Photo( flightDto.Aircraft.Photo.Name, flightDto.Aircraft.Photo.UrlPath, flightDto.Aircraft.Photo.IsDeleted )
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
                   o.FreePlace.Equals(o.FreePlace) &&
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