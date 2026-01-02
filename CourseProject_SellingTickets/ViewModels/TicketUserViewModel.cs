using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject_SellingTickets.Commands.TicketCommands;
using CourseProject_SellingTickets.Interfaces.TicketProviderInterface;
using CourseProject_SellingTickets.Models;
using DynamicData.Binding;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class TicketUserViewModel : ViewModelBase
{
    private readonly ITicketVmProvider? _ticketProvider;

    //Observable properties 
    
    // => // Combobox Collections
    
    private ObservableCollection<Flight>? _flights;
    public ObservableCollection<Flight> Flights => _flights ??= new ObservableCollection<Flight>();

    private ObservableCollection<Discount>? _discounts;
    public ObservableCollection<Discount> Discounts => _discounts ??= new ObservableCollection<Discount>();

    private ObservableCollection<FlightClass>? _flightClasses;
    public ObservableCollection<FlightClass> FlightClasses => _flightClasses ??= new ObservableCollection<FlightClass>();

    private ObservableCollection<User>? _users;
    public ObservableCollection<User> Users => _users ??= new ObservableCollection<User>();
    
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
    
    private Ticket? _selectedTicket;
    public Ticket SelectedTicket { get => _selectedTicket!; set => this.RaiseAndSetIfChanged(ref _selectedTicket, value); }

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
    
    private ObservableCollectionExtended<Ticket>? _ticketItems;
    public ObservableCollectionExtended<Ticket> TicketItems => _ticketItems ??= new ObservableCollectionExtended<Ticket>();
    
    // Commands 
    
    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand => _hideSideBarCommand ??= ReactiveCommand.Create<Unit>(_ => SideBarShowed = false);

    private ICommand? _purgeSelectedUser;
    public ICommand PurgeSelectedUser => _purgeSelectedUser ??= ReactiveCommand.Create<Unit>(_ => SelectedTicket.User = new User());

    private ReactiveCommand<bool, Unit>? _addEditDataCommand;
    public ReactiveCommand<bool, Unit> AddEditDataCommand => _addEditDataCommand ??= new AddEditTicketCommand(this);
    
    private ReactiveCommand<IEnumerable<Ticket>, Task>? _loadTicketDataCommand;
    public ReactiveCommand<IEnumerable<Ticket>, Task> LoadTicketDataCommand => _loadTicketDataCommand ??= new LoadTicketDataCommand(this, _ticketProvider!);

    private ReactiveCommand<Unit, Task<IEnumerable<Ticket>?>>? _searchTicketDataCommand;
    public ReactiveCommand<Unit, Task<IEnumerable<Ticket>?>> SearchTicketDataCommand => _searchTicketDataCommand ??= new SearchTicketDataCommand(this, _ticketProvider!);

    private ReactiveCommand<Unit, Task>? _saveTicketDataCommand;
    public ReactiveCommand<Unit, Task> SaveTicketDataCommand => _saveTicketDataCommand ??= new SaveTicketDataCommand(this, _ticketProvider!);

    private ReactiveCommand<Unit, Task>? _deleteTicketDataCommand;
    public ReactiveCommand<Unit, Task> DeleteTicketDataCommand => _deleteTicketDataCommand ??= new DeleteTicketDataCommand(this, _ticketProvider!);


    // Constructor
    
    public TicketUserViewModel(ITicketVmProvider? ticketVmProvider)
    {
        _ticketProvider = ticketVmProvider;
        
        LoadTicketDataCommand.Execute();
        SearchTicketDataCommand.Subscribe(filteredTickets => LoadTicketDataCommand!.Execute(filteredTickets.Result!));
        
        ConnectionDbState.CheckConnectionState
            .Subscribe(isConnected => DatabaseHasConnected = isConnected.Result);
    }
}
