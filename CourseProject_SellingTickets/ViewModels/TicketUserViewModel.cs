using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject_SellingTickets.Commands;
using CourseProject_SellingTickets.Commands.TicketCommands;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.TicketProvider;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class TicketUserViewModel : ViewModelBase
{
    private ITicketVmProvider? _ticketProvider;
    private IConnectionStateProvider? _connectionStateProvider;

    //Observable properties 
    
    // => // Combobox Collections
    
    private ObservableCollection<Flight>? _flights;
    public ObservableCollection<Flight> Flights => _flights ??= new ObservableCollection<Flight>();

    private ObservableCollection<Discount>? _discounts;
    public ObservableCollection<Discount> Discounts => _discounts ??= new ObservableCollection<Discount>();

    private ObservableCollection<FlightClass>? _flightClasses;
    public ObservableCollection<FlightClass> FlightClasses => _flightClasses ??= new ObservableCollection<FlightClass>();
    
    // => // Filters
    
    private bool _sideBarShowed = true;
    public bool SideBarShowed { get => _sideBarShowed; set => this.RaiseAndSetIfChanged(ref _sideBarShowed, value); }
    
    // => // Filters => // Search Terms
    
    private string? _searchTerm;
    public string? SearchTerm { get => _searchTerm; set { this.RaiseAndSetIfChanged(ref _searchTerm, value); this.RaisePropertyChanged(nameof(HasSearching)); } }
    public bool HasSearching => !string.IsNullOrEmpty(SearchTerm);
    
    private int _limitRows = 50;
    public int LimitRows { get => _limitRows; set => this.RaiseAndSetIfChanged(ref _limitRows, value); }
    
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
    
    // => // ObservableCollection
    
    private ObservableCollection<Ticket>? _ticketItems;
    public ObservableCollection<Ticket> TicketItems => _ticketItems ??= new ObservableCollection<Ticket>();
    
    // Commands 
    
    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand => _hideSideBarCommand ??= ReactiveCommand.Create<Unit>(_ => SideBarShowed = false);

    private ReactiveCommand<bool, Unit>? _addEditTicketCommand;
    public ReactiveCommand<bool, Unit> AddEditTicketCommand => _addEditTicketCommand ??= new AddEditTicketCommand(this);
    
    private ReactiveCommand<IEnumerable<Ticket>, Task>? _loadTicketDataCommand;
    public ReactiveCommand<IEnumerable<Ticket>, Task> LoadTicketDataCommand => _loadTicketDataCommand ??= new LoadTicketDataCommand(this, _ticketProvider!, _connectionStateProvider!);
    
    // Constructor
    
    public TicketUserViewModel(ITicketVmProvider? ticketVmProvider, IConnectionStateProvider? connectionStateProvider)
    {
        _ticketProvider = ticketVmProvider;
        _connectionStateProvider = connectionStateProvider;
        
        LoadTicketDataCommand!.Execute();
    }
    
}