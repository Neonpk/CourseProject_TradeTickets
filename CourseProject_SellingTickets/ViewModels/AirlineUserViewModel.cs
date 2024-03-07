using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject_SellingTickets.Commands.AirlineCommands;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.AirlineProvider;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class AirlineUserViewModel : ViewModelBase
{
    private readonly IAirlineVmProvider _airlineVmProvider;
    private readonly IConnectionStateProvider _connectionStateProvider;
    
    //Observable properties 
    
    // => // Filters
    
    private bool _sideBarShowed;
    public bool SideBarShowed { get => _sideBarShowed; set => this.RaiseAndSetIfChanged(ref _sideBarShowed, value); }
    
    // => // Filters => // Search Terms
    
    private string? _searchTerm;
    public string? SearchTerm { get => _searchTerm; set { this.RaiseAndSetIfChanged(ref _searchTerm, value); this.RaisePropertyChanged(nameof(HasSearching)); } }
    public bool HasSearching => !string.IsNullOrEmpty(SearchTerm);
    
    private int _limitRows = 50;
    public int LimitRows { get => _limitRows; set => this.RaiseAndSetIfChanged(ref _limitRows, value); }

    private int _selectedSearchMode;
    public int SelectedSearchMode { get => _selectedSearchMode; set => this.RaiseAndSetIfChanged(ref _selectedSearchMode, value); }
    
    // Selected Model from the list
    
    private Airline? _selectedAirline;
    public Airline SelectedAirline { get => _selectedAirline!; set => this.RaiseAndSetIfChanged(ref _selectedAirline, value); }
    
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
    
    // => // ObservableCollection And SourceCache
    
    private ObservableCollection<Airline>? _airlineItems;
    public ObservableCollection<Airline> AirlineItems => _airlineItems ??= new ObservableCollection<Airline>();
    
    // Commands 
    
    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand => _hideSideBarCommand ??= ReactiveCommand.Create<Unit>(_ => SideBarShowed = false);

    private ReactiveCommand<bool, Unit>? _addEditDataCommand;
    public ReactiveCommand<bool, Unit> AddEditDataCommand => _addEditDataCommand ??= new AddEditAirlineCommand(this);
    
    private ReactiveCommand<IEnumerable<Airline>, Task>? _loadAirlineDataCommand;
    public ReactiveCommand<IEnumerable<Airline>, Task> LoadAirlineDataCommand => _loadAirlineDataCommand ??= new LoadAirlineDataCommand(this, _airlineVmProvider!, _connectionStateProvider!);

    private ReactiveCommand<Unit, Task<IEnumerable<Airline>?>>? _searchAirlineDataCommand;
    public ReactiveCommand<Unit, Task<IEnumerable<Airline>?>> SearchAirlineDataCommand => _searchAirlineDataCommand ??= new SearchAirlineDataCommand(this, _airlineVmProvider!);

    private ReactiveCommand<Unit, Task>? _saveAirlineDataCommand;
    public ReactiveCommand<Unit, Task> SaveAirlineDataCommand => _saveAirlineDataCommand ??= new SaveAirlineDataCommand(this, _airlineVmProvider!, _connectionStateProvider!);

    private ReactiveCommand<Unit, Task>? _deleteAirlineDataCommand;
    public ReactiveCommand<Unit, Task> DeleteAirlineDataCommand => _deleteAirlineDataCommand ??= new DeleteAirlineDataCommand(this, _airlineVmProvider!, _connectionStateProvider!);
    
    
    // Constructor 
    
    public AirlineUserViewModel(IAirlineVmProvider? airlineVmProvider, IConnectionStateProvider? connectionStateProvider)
    {
        _airlineVmProvider = airlineVmProvider!;
        _connectionStateProvider = connectionStateProvider!;
        
        LoadAirlineDataCommand.Execute();
        SearchAirlineDataCommand.Subscribe(filteredAirlines => LoadAirlineDataCommand!.Execute(filteredAirlines.Result!));
    }
}