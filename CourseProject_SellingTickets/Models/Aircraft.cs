using System;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReactiveUI;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Aircraft : ReactiveObject
{
    // Main Model

    private System.Int64 _id;
    public System.Int64 Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }
    
    private string _model;
    public string Model { get => _model; set => this.RaiseAndSetIfChanged(ref _model, value); }

    private string _type;
    public string Type { get => _type; set => this.RaiseAndSetIfChanged(ref _type, value); }

    private int _totalPlace;
    public int TotalPlace { get => _totalPlace; set => this.RaiseAndSetIfChanged(ref _totalPlace, value); }

    private Photo _photo;
    public Photo Photo { get => _photo; set => this.RaiseAndSetIfChanged(ref _photo, value); }

    public Aircraft()
    {
        Model = String.Empty;
        Type = String.Empty;
        Photo = new Photo();
    }
    
    public Aircraft(System.Int64 id, string model, string type, int totalPlace, Photo photo)
    {
        Id = id;
        Model = model;
        Type = type;
        TotalPlace = totalPlace;
        Photo = photo;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is Aircraft o)
        {
            return o.Id.Equals(Id) && 
                   o.Model!.Equals(Model) &&
                   o.Type!.Equals(Type) && 
                   o.TotalPlace.Equals(TotalPlace) &&
                   o.Photo.Equals(Photo);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}