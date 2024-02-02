using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CourseProject_SellingTickets.Helpers;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Flight
{
    // Private 

    // Main Model 

    public System.Int64? Id { get; init; }
    public System.Int64 FlightNumber { get; set; }
    public Place DeparturePlace { get; set; }
    public DateTime DepartureTime { get; set; }
    public Place DestinationPlace { get; set; }
    public DateTime ArrivalTime { get; set; }

    public Aircraft Aircraft { get; set;  }
    public int TotalPlace { get; }
    public int FreePlace { get; }
    public System.TimeSpan DurationTime { get; }

    public Airline Airline { get; set; }
    public bool IsCanceled { get; set; }
    
    // Custom Properties
    public Task<Bitmap?> DeparturePlaceImage { get => ImageHelper.LoadFromWeb(new Uri(DeparturePlace.Photo.UrlPath)); }
    public Task<Bitmap?> DestinationPlaceImage { get => ImageHelper.LoadFromWeb(new Uri(DestinationPlace.Photo.UrlPath)); }
    public Task<Bitmap?> AircraftImage { get => ImageHelper.LoadFromWeb(new Uri(Aircraft.Photo.UrlPath)); }
    
    //Constructor
    
    public Flight() {}
    
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