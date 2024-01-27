using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using CourseProject_SellingTickets.Commands;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class FlightUserViewModel : ViewModelBase
{
    // Private 

    private FlightProvider? _flightProvider;

    //Observable properties 
    
    // => // Combobox Collections

    private List<Aircraft>? _aircrafts;
    public List<Aircraft> Aircrafts { get => _aircrafts ??= new List<Aircraft>(); set { _aircrafts = value; OnPropertyChanged(nameof(Aircrafts)); } }
    
    private List<Airline>? _airlines;
    public List<Airline> Airlines { get => _airlines ??= new List<Airline>(); set { _airlines = value; OnPropertyChanged(nameof(Airlines)); } }
    
    private List<Place>? _places;
    public List<Place> Places { get => _places ??= new List<Place>(); set { _places = value; OnPropertyChanged(nameof(Places)); } }
    
    // => // Filters
        
    private bool _sideBarShowed;
    public bool SideBarShowed { get => _sideBarShowed; set { _sideBarShowed = value; OnPropertyChanged(nameof(SideBarShowed)); } }

    private string? _textBoxFilter;
    public string? TextFilter { get => _textBoxFilter; set { _textBoxFilter = value; OnPropertyChanged(nameof(TextFilter)); } }

    private Flight? _selectedFlight;
    public Flight? SelectedFlight { get => _selectedFlight; set { _selectedFlight = value; OnPropertyChanged(nameof(SelectedFlight)); } }
    
    // => // Loading page properties 
    
    private bool? _isLoading;
    public bool? IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

    private string? _errorMessage;
    public string? ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); } }
    public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    
    // => // ObservableCollection
    
    private ObservableCollection<Flight>? _flightsItems;
    public ObservableCollection<Flight>? FlightItems { get => _flightsItems; set { _flightsItems = value; OnPropertyChanged(nameof(FlightItems)); } }
    
    // Commands 

    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand { get => _hideSideBarCommand ??= ReactiveCommand.Create<object>((_) => SideBarShowed = false); }
    
    private ReactiveCommand<Unit, Unit>? _loadFLightsCommand;
    public ReactiveCommand<Unit,Unit>? LoadFlightsCommand { get => _loadFLightsCommand ??= new LoadFlightsCommand(this, _flightProvider!); }
    
    private ReactiveCommand<bool,Unit>? _addEditDataCommand;
    public ReactiveCommand<bool,Unit>? AddEditDataCommand { get => _addEditDataCommand ??= new AddEditFlightCommand(this, _flightProvider!); }
    
    // Constructor 
    public FlightUserViewModel(FlightProvider? flightProvider)
    {
        _flightProvider = flightProvider;
        
        FlightItems = new ObservableCollection<Flight>();
        LoadFlightsCommand!.Execute();
    }

}