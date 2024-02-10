using System;
using CourseProject_SellingTickets.ViewModels;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Aircraft : ObservableObject
{
    // Main Model

    private System.Int64? _id;
    public System.Int64? Id { get => _id; set { _id = value; OnPropertyChanged(nameof(Id)); } }
    
    private string _model;
    public string Model { get => _model; set { _model = value; OnPropertyChanged(nameof(Model)); } }

    private string _type;
    public string Type { get => _type; set { _type = value; OnPropertyChanged(nameof(Type)); } }

    private int _totalPlace;
    public int TotalPlace { get => _totalPlace; set { _totalPlace = value; OnPropertyChanged(nameof(TotalPlace)); } }

    private Photo _photo;
    public Photo Photo { get => _photo; set { _photo = value; OnPropertyChanged(nameof(Photo)); } }

    public Aircraft()
    {
        Id = null;
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