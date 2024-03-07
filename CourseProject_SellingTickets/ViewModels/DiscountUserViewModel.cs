using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject_SellingTickets.Commands.DiscountCommands;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.DiscountProvider;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class DiscountUserViewModel : ViewModelBase
{
    private readonly IDiscountVmProvider _discountVmProvider;
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
    
    private Discount? _selectedDiscount;
    public Discount SelectedDiscount { get => _selectedDiscount!; set => this.RaiseAndSetIfChanged(ref _selectedDiscount, value); }
    
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
    
    private ObservableCollection<Discount>? _discountItems;
    public ObservableCollection<Discount> DiscountItems => _discountItems ??= new ObservableCollection<Discount>();
    
    // Commands 
    
    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand => _hideSideBarCommand ??= ReactiveCommand.Create<Unit>(_ => SideBarShowed = false);

    private ReactiveCommand<bool, Unit>? _addEditDataCommand;
    public ReactiveCommand<bool, Unit> AddEditDataCommand => _addEditDataCommand ??= new AddEditDiscountCommand(this);
    
    private ReactiveCommand<IEnumerable<Discount>, Task>? _loadDiscountDataCommand;
    public ReactiveCommand<IEnumerable<Discount>, Task> LoadDiscountDataCommand => _loadDiscountDataCommand ??= new LoadDiscountDataCommand(this, _discountVmProvider!, _connectionStateProvider!);

    private ReactiveCommand<Unit, Task<IEnumerable<Discount>?>>? _searchDiscountDataCommand;
    public ReactiveCommand<Unit, Task<IEnumerable<Discount>?>> SearchDiscountDataCommand => _searchDiscountDataCommand ??= new SearchDiscountDataCommand(this, _discountVmProvider!);

    private ReactiveCommand<Unit, Task>? _saveDiscountDataCommand;
    public ReactiveCommand<Unit, Task> SaveDiscountDataCommand => _saveDiscountDataCommand ??= new SaveDiscountDataCommand(this, _discountVmProvider!, _connectionStateProvider!);

    private ReactiveCommand<Unit, Task>? _deleteDiscountDataCommand;
    public ReactiveCommand<Unit, Task> DeleteDiscountDataCommand => _deleteDiscountDataCommand ??= new DeleteDiscountDataCommand(this, _discountVmProvider!, _connectionStateProvider!);
    
    // Constructor 
    
    public DiscountUserViewModel(IDiscountVmProvider? discountVmProvider, IConnectionStateProvider? connectionStateProvider)
    {
        _discountVmProvider = discountVmProvider!;
        _connectionStateProvider = connectionStateProvider!;
        
        LoadDiscountDataCommand.Execute();
        SearchDiscountDataCommand.Subscribe(filteredDiscounts => LoadDiscountDataCommand!.Execute(filteredDiscounts.Result!));
    }
}