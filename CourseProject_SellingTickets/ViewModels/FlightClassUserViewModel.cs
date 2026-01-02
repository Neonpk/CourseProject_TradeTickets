using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject_SellingTickets.Commands.FlightClassCommands;
using CourseProject_SellingTickets.Interfaces.FlightClassProviderInterface;
using CourseProject_SellingTickets.Models;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class FlightClassUserViewModel : ViewModelBase
{
    private readonly IFlightClassVmProvider _flightClassVmProvider;
    
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
    
    private FlightClass? _selectedFlightClass;
    public FlightClass SelectedFlightClass { get => _selectedFlightClass!; set => this.RaiseAndSetIfChanged(ref _selectedFlightClass, value); }
    
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
    
    private ObservableCollection<FlightClass>? _flightClassItems;
    public ObservableCollection<FlightClass> FlightClassItems => _flightClassItems ??= new ObservableCollection<FlightClass>();
    
    // Commands 
    
    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand => _hideSideBarCommand ??= ReactiveCommand.Create<Unit>(_ => SideBarShowed = false);

    private ReactiveCommand<bool, Unit>? _addEditDataCommand;
    public ReactiveCommand<bool, Unit> AddEditDataCommand => _addEditDataCommand ??= new AddEditFlightClassCommand(this);
    
    private ReactiveCommand<IEnumerable<FlightClass>, Task>? _loadFlightClassDataCommand;
    public ReactiveCommand<IEnumerable<FlightClass>, Task> LoadFlightClassDataCommand => _loadFlightClassDataCommand ??= new LoadFlightClassDataCommand(this, _flightClassVmProvider!);

    private ReactiveCommand<Unit, Task<IEnumerable<FlightClass>?>>? _searchFlightClassDataCommand;
    public ReactiveCommand<Unit, Task<IEnumerable<FlightClass>?>> SearchFlightClassDataCommand => _searchFlightClassDataCommand ??= new SearchFlightClassDataCommand(this, _flightClassVmProvider);

    private ReactiveCommand<Unit, Task>? _saveFlightClassDataCommand;
    public ReactiveCommand<Unit, Task> SaveFlightClassDataCommand => _saveFlightClassDataCommand ??= new SaveFlightClassDataCommand(this, _flightClassVmProvider!);

    private ReactiveCommand<Unit, Task>? _deleteFlightClassDataCommand;
    public ReactiveCommand<Unit, Task> DeleteFlightClassDataCommand => _deleteFlightClassDataCommand ??= new DeleteFlightClassDataCommand(this, _flightClassVmProvider!);
    
    
    // Constructor 
    
    public FlightClassUserViewModel(IFlightClassVmProvider? flightClassVmProvider)
    {
        _flightClassVmProvider = flightClassVmProvider!;
        
        LoadFlightClassDataCommand.Execute();
        SearchFlightClassDataCommand.Subscribe(filteredFlightClasses => LoadFlightClassDataCommand!.Execute(filteredFlightClasses.Result!));
        
        ConnectionDbState.CheckConnectionState.Subscribe(isConnected => DatabaseHasConnected = isConnected.Result);
    }
}
