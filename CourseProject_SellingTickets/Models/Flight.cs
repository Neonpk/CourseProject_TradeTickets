using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Flight
{
    // Private 

    // Main Model 

    public System.Int64 Id { get; }
    public System.Int64 FlightNumber { get; }
    public Place DeparturePlace { get; }
    public System.DateTime DepartureTime { get; }
    public Place DestinationPlace { get; }
    public System.DateTime ArrivalTime { get; }

    public Aircraft Aircraft { get; }
    public int TotalPlace { get; }
    public int FreePlace { get; }
    public System.TimeSpan DurationTime { get; }

    public Airline Airline { get; }
    public bool IsCanceled { get; }
    
    // Custom Properties
    public string FullInfoDeparturePlace { get => $"{DeparturePlace.Name} - {DeparturePlace.Description}"; }
    public string FullInfoDestinationPlace { get => $"{DestinationPlace.Name} - {DestinationPlace.Description}"; }
    
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
    
}