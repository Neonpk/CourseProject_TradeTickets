using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject_SellingTickets.Commands.AircraftCommands;
using CourseProject_SellingTickets.Interfaces.AircraftProviderInterface;
using CourseProject_SellingTickets.Models;
using DynamicData.Binding;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class AircraftUserViewModel : ViewModelBase
{
    // Private
    
    private readonly IAircraftVmProvider? _aircraftVmProvider;
    
    //Observable properties 
    
    // => // Combobox Collections
    
    private ObservableCollection<Photo>? _photos;
    public ObservableCollection<Photo> Photos => _photos ??= new ObservableCollection<Photo>();
    
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
    
    private Aircraft? _selectedAircraft;
    public Aircraft SelectedAircraft { get => _selectedAircraft!; set => this.RaiseAndSetIfChanged(ref _selectedAircraft, value); }

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
    
    private ObservableCollection<Aircraft>? _aircraftItems;
    public ObservableCollection<Aircraft> AircraftItems => _aircraftItems ??= new ObservableCollection<Aircraft>();
    
    // Commands 

    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand => _hideSideBarCommand ??= ReactiveCommand.Create<Unit>((_) => SideBarShowed = false);

    private ReactiveCommand<IEnumerable<Aircraft>, Task>? _loadAircraftDataCommand;
    public ReactiveCommand<IEnumerable<Aircraft>, Task> LoadAircraftDataCommand => _loadAircraftDataCommand ??= new LoadAircraftDataCommand(this, _aircraftVmProvider!);

    private ReactiveCommand<bool,Unit>? _addEditDataCommand;
    public ReactiveCommand<bool,Unit> AddEditDataCommand => _addEditDataCommand ??= new AddEditAircraftDataCommand(this!);

    private ReactiveCommand<Unit, Task>? _saveAircraftDataCommand;
    public ReactiveCommand<Unit, Task> SaveAircraftDataCommand => _saveAircraftDataCommand ??= new SaveAircraftDataCommand(this, _aircraftVmProvider!);

    private ReactiveCommand<Unit, Task>? _deleteAircraftDataCommand;
    public ReactiveCommand<Unit, Task>? DeleteAircraftDataCommand => _deleteAircraftDataCommand ??= new DeleteAircraftDataCommand(this, _aircraftVmProvider!);

    private ReactiveCommand<Unit, Unit>? _sortAircraftCommand;
    public ReactiveCommand<Unit, Unit> SortAircraftCommand => _sortAircraftCommand ??= new SortAircraftsCommand(this);

    private ReactiveCommand<Unit, Task<IEnumerable<Aircraft>?>>? _searchAircraftDataCommand;
    public ReactiveCommand<Unit, Task<IEnumerable<Aircraft>?>> SearchAircraftDataCommand => _searchAircraftDataCommand ??= new SearchAircraftDataCommand(this!, _aircraftVmProvider!);
    
    public AircraftUserViewModel(IAircraftVmProvider? aircraftVmProvider)
    {
        _aircraftVmProvider = aircraftVmProvider;
        
        LoadAircraftDataCommand.Execute();
        SearchAircraftDataCommand.Subscribe(filteredAircrafts => LoadAircraftDataCommand.Execute(filteredAircrafts.Result!));
        
        this.WhenAnyPropertyChanged([nameof(SelectedSortMode), nameof(SelectedSortValue)]).
            Subscribe(x => SortAircraftCommand!.Execute());
        
        ConnectionDbState.CheckConnectionState.Subscribe(isConnected => DatabaseHasConnected = isConnected.Result);
    }
}
