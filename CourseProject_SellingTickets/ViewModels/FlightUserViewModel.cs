using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using CourseProject_SellingTickets.Commands;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class FlightUserViewModel : ViewModelBase
{
    // Private 

    private IFlightVmProvider? _flightProvider;
    private IConnectionStateProvider? _connectionStateProvider;

    //Observable properties 
    
    // => // Combobox Collections
    
    private ObservableCollection<Aircraft>? _aircrafts;
    public ObservableCollection<Aircraft> Aircrafts { get => _aircrafts ??= new ObservableCollection<Aircraft>(); }
    
    private ObservableCollection<Airline>? _airlines;
    public ObservableCollection<Airline> Airlines { get => _airlines ??= new ObservableCollection<Airline>(); }
    
    private ObservableCollection<Place>? _places;
    public ObservableCollection<Place> Places { get => _places ??= new ObservableCollection<Place>(); }
    
    // => // Filters
        
    private bool _sideBarShowed;
    public bool SideBarShowed { get => _sideBarShowed; set { _sideBarShowed = value; OnPropertyChanged(nameof(SideBarShowed)); } }

    private string? _searchTerm;
    public string? SearchTerm { get => _searchTerm; set { _searchTerm = value; OnPropertyChanged(nameof(SearchTerm)); OnPropertyChanged(nameof(HasSearching)); } }
    public bool HasSearching => !string.IsNullOrEmpty(SearchTerm);

    private int _limitRows = 50;
    public int LimitRows { get => _limitRows; set { _limitRows = value; OnPropertyChanged(nameof(LimitRows)); } }
    
    // => // Filters // => // Sort Modes

    private int _selectedSortValue;
    public int SelectedSortValue { get => _selectedSortValue; set { _selectedSortValue = value; OnPropertyChanged(nameof(SelectedSortValue)); } }

    private int _selectedSortMode;
    public int SelectedSortMode { get => _selectedSortMode; set { _selectedSortMode = value; OnPropertyChanged(nameof(SelectedSortMode)); } }
    
    // => // Filters => // Search Terms

    private int _selectedSearchMode;
    public int SelectedSearchMode { get => _selectedSearchMode; set { _selectedSearchMode = value; OnPropertyChanged(nameof(SelectedSearchMode)); } }

    // Selected Model from the list
    
    private Flight? _selectedFlight;
    public Flight? SelectedFlight { get => _selectedFlight; set { _selectedFlight = value; OnPropertyChanged(nameof(SelectedFlight)); } }
    
    // => // Loading page properties 

    private bool? _databaseHasConnected;
    public bool? DatabaseHasConnected { get => _databaseHasConnected; set { _databaseHasConnected = value; OnPropertyChanged(nameof(DatabaseHasConnected)); } }
    
    private bool? _isLoading;
    public bool? IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

    private bool? _isLoadingEditMode;
    public bool? IsLoadingEditMode { get => _isLoadingEditMode; set { _isLoadingEditMode = value; OnPropertyChanged(nameof(IsLoadingEditMode)); } }
    
    private string? _errorMessage;
    public string? ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); OnPropertyChanged(nameof(HasErrorMessage)); } }
    public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    public bool HasFlights => _flightsItems!.Any();
    
    // => // ObservableCollection
    
    private ObservableCollection<Flight>? _flightsItems;
    public ObservableCollection<Flight>? FlightItems { get => _flightsItems ??= new ObservableCollection<Flight>(); }
    
    // Commands 

    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand { get => _hideSideBarCommand ??= ReactiveCommand.Create<Unit>((_) => SideBarShowed = false); }

    private ReactiveCommand<Unit, Unit>? _loadFlightsCommand;
    public ReactiveCommand<Unit,Unit>? LoadFlightsCommand { get => 
        _loadFlightsCommand ??= new LoadFlightsCommand(this, _flightProvider!, _connectionStateProvider!); }
    
    private ReactiveCommand<bool,Unit>? _addEditDataCommand;
    public ReactiveCommand<bool,Unit>? AddEditDataCommand { get => _addEditDataCommand ??= new AddEditFlightCommand(this!); }

    private ReactiveCommand<Unit, Unit>? _saveFlightDataCommand;
    public ReactiveCommand<Unit, Unit>? SaveFlightDataCommand { get => _saveFlightDataCommand ??= new SaveFlightDataCommand(this, _flightProvider!); }

    private ReactiveCommand<Unit, Unit>? _deleteFlightDataCommand;
    public ReactiveCommand<Unit, Unit>? DeleteFlightDataCommand { get => _deleteFlightDataCommand ??= new DeleteFlightDataCommand(this, _flightProvider!); }

    private ReactiveCommand<Unit, Unit>? _sortFlightsCommand;
    public ReactiveCommand<Unit, Unit>? SortFlightsCommand { get => _sortFlightsCommand ??= new SortFlightsCommand(this); } 
    
    private ReactiveCommand<bool, IEnumerable<Flight>?>? _searchTermFlightCommand;
    public ReactiveCommand<bool, IEnumerable<Flight>?>? SearchTermFlightCommand { get => _searchTermFlightCommand ??= new SearchTermFlightCommand(this, _flightProvider!)!; } 
    
    // Constructor 
    public FlightUserViewModel(IFlightVmProvider? flightProvider, IConnectionStateProvider? connectionStateProvider)
    {
        _flightProvider = flightProvider;
        _connectionStateProvider = connectionStateProvider;
        
        LoadFlightsCommand!.Execute();

        this.WhenAnyPropertyChanged([nameof(SelectedSortMode), nameof(SelectedSortValue)]).
            Subscribe(x => SortFlightsCommand!.Execute());
    }

}