using System;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class Photo : ReactiveObject
{
    // Main Model

    private System.Int64 _id;
    public System.Int64 Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }

    private string _name;
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }

    private string _urlPath;
    public string UrlPath { get => _urlPath; set => this.RaiseAndSetIfChanged(ref _urlPath, value); }

    private bool _isDeleted;
    public bool IsDeleted { get => _isDeleted; set => this.RaiseAndSetIfChanged(ref _isDeleted, value); }

    public Photo()
    {
        Name = String.Empty;
        UrlPath = String.Empty;
    }
    
    public Photo(long id, string name, string urlPath, bool isDeleted)
    {
        Id = id;
        Name = name;
        UrlPath = urlPath;
        IsDeleted = isDeleted;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Photo o)
        {
            return Id.Equals(o.Id) &&
                   Name.Equals(o.Name) &&
                   UrlPath.Equals(o.UrlPath) &&
                   IsDeleted.Equals(o.IsDeleted);
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}