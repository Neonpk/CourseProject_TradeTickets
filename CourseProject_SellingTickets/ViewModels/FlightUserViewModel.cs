using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject_SellingTickets.Commands.FlightCommands;
using CourseProject_SellingTickets.Interfaces;
using CourseProject_SellingTickets.Interfaces.FlightProviderInterface;
using CourseProject_SellingTickets.Models;
using DynamicData.Binding;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class FlightUserViewModel : ViewModelBase, IParameterReceiver
{
    // Custom parameters for viewModel 

    private Int64 _userId;
    public Int64 UserId { get => _userId; private set => this.RaiseAndSetIfChanged(ref _userId, value); }
    
    // Private 

    private readonly IFlightVmProvider? _flightProvider;

    //Observable properties 
    
    // => // Combobox Collections
    
    private ObservableCollection<Aircraft>? _aircrafts;
    public ObservableCollection<Aircraft> Aircrafts => _aircrafts ??= new ObservableCollection<Aircraft>();

    private ObservableCollection<Airline>? _airlines;
    public ObservableCollection<Airline> Airlines => _airlines ??= new ObservableCollection<Airline>();

    private ObservableCollection<Place>? _places;
    public ObservableCollection<Place> Places => _places ??= new ObservableCollection<Place>();

    // => // Filters
        
    private bool _sideBarShowed;
    public bool SideBarShowed { get => _sideBarShowed; set => this.RaiseAndSetIfChanged(ref _sideBarShowed, value); }

    private string? _searchTerm;
    public string? SearchTerm { get => _searchTerm; set { this.RaiseAndSetIfChanged(ref _searchTerm, value); this.RaisePropertyChanged(nameof(HasSearching)); } }
    public bool HasSearching => !string.IsNullOrEmpty(SearchTerm);

    private int _limitRows = 50;
    public int LimitRows { get => _limitRows; set => this.RaiseAndSetIfChanged(ref _limitRows, value); }
    
    // => // Filters // => // Sort Modes

    private int _selectedSortValue;
    public int SelectedSortValue { get => _selectedSortValue; set => this.RaiseAndSetIfChanged(ref _selectedSortValue, value); }

    private int _selectedSortMode;
    public int SelectedSortMode { get => _selectedSortMode; set => this.RaiseAndSetIfChanged(ref _selectedSortMode, value); }
    
    // => // Filters => // Search Terms

    private int _selectedSearchMode;
    public int SelectedSearchMode { get => _selectedSearchMode; set => this.RaiseAndSetIfChanged(ref _selectedSearchMode, value); }

    // Selected Model from the list
    
    private Flight? _selectedFlight;
    public Flight SelectedFlight { get => _selectedFlight!; set => this.RaiseAndSetIfChanged(ref _selectedFlight, value); }

    // => // Loading page properties 

    private bool _databaseHasConnected;
    public bool DatabaseHasConnected { get => _databaseHasConnected; set => this.RaiseAndSetIfChanged(ref _databaseHasConnected, value); }

    private bool? _isLoading;
    public bool? IsLoading { get => _isLoading; set => this.RaiseAndSetIfChanged(ref _isLoading, value); }

    private bool? _isLoadingEditMode;
    public bool? IsLoadingEditMode { get => _isLoadingEditMode; set => this.RaiseAndSetIfChanged(ref _isLoadingEditMode, value); }

    private string? _errorMessage;
    public string ErrorMessage { get => _errorMessage!; set { this.RaiseAndSetIfChanged(ref _errorMessage, value); this.RaisePropertyChanged(nameof(HasErrorMessage)); } }

    public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    
    // => // ObservableCollection
    
    private ObservableCollection<Flight>? _flightsItems;
    public ObservableCollection<Flight> FlightItems => _flightsItems ??= new ObservableCollection<Flight>();

    // Commands 

    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand => _hideSideBarCommand ??= ReactiveCommand.Create<Unit>((_) => SideBarShowed = false);

    private ReactiveCommand<IEnumerable<Flight>, Task>? _loadFlightDataCommand;
    public ReactiveCommand<IEnumerable<Flight>, Task> LoadFlightDataCommand => _loadFlightDataCommand ??= new LoadFlightDataCommand(this, _flightProvider!);

    private ReactiveCommand<bool,Unit>? _addEditDataCommand;
    public ReactiveCommand<bool,Unit> AddEditDataCommand => _addEditDataCommand ??= new AddEditFlightCommand(this!);

    private ReactiveCommand<Unit, Task>? _saveFlightDataCommand;
    public ReactiveCommand<Unit, Task> SaveFlightDataCommand => _saveFlightDataCommand ??= new SaveFlightDataCommand(this, _flightProvider!);

    private ReactiveCommand<Unit, Task>? _deleteFlightDataCommand;
    public ReactiveCommand<Unit, Task>? DeleteFlightDataCommand => _deleteFlightDataCommand ??= new DeleteFlightDataCommand(this, _flightProvider!);

    private ReactiveCommand<Unit, Unit>? _sortFlightsCommand;
    public ReactiveCommand<Unit, Unit> SortFlightsCommand => _sortFlightsCommand ??= new SortFlightsCommand(this);

    private ReactiveCommand<Unit, Task<IEnumerable<Flight>?>>? _searchFlightDataCommand;
    public ReactiveCommand<Unit, Task<IEnumerable<Flight>?>> SearchFlightDataCommand => _searchFlightDataCommand ??= new SearchFlightDataCommand(this, _flightProvider!)!;

    // Constructor 
    public FlightUserViewModel(IFlightVmProvider? flightProvider)
    {
        _flightProvider = flightProvider;

        LoadFlightDataCommand.Execute();
        SearchFlightDataCommand.Subscribe(filteredFlights => LoadFlightDataCommand.Execute(filteredFlights.Result!));
        ConnectionDbState.CheckConnectionState.Subscribe(isConnected => DatabaseHasConnected = isConnected.Result);
        
        this.WhenAnyPropertyChanged([nameof(SelectedSortMode), nameof(SelectedSortValue)]).
            Subscribe(x => SortFlightsCommand!.Execute());
    }

    // Receive Navigation Paramater
    
    public void ReceieveParameter(object parameter)
    {
        UserId = parameter is Int64 param ? param : -1;
        
        SearchFlightDataCommand.Execute().Subscribe();
    }
}
