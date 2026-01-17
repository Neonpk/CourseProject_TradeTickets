using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject_SellingTickets.Commands.UserListCommands;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class UserListViewModel : ViewModelBase
{
    private readonly IUserListVmProvider _userListVmProvider;
    
    //Observable properties 
    
    // => // Combobox Collections
    
    private ObservableCollection<Discount>? _discounts;
    public ObservableCollection<Discount> Discounts => _discounts ??= new ObservableCollection<Discount>();
    
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
    
    private User? _selectedUser;
    public User SelectedUser { get => _selectedUser!; set => this.RaiseAndSetIfChanged(ref _selectedUser, value); }
    
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
    
    private ObservableCollection<User>? _userItems;
    public ObservableCollection<User> UserItems => _userItems ??= new ObservableCollection<User>();
    
    // Commands 
    
    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand => 
        _hideSideBarCommand ??= ReactiveCommand.Create<Unit>(_ => SideBarShowed = false);
    
    private ReactiveCommand<Unit, Unit>? _editDataCommand;
    public ReactiveCommand<Unit, Unit> EditDataCommand =>
        _editDataCommand ??= ReactiveCommand.Create<Unit>(_ => SideBarShowed = true);
    
    private ReactiveCommand<IEnumerable<User>, Task>? _loadUserDataCommand;
    public ReactiveCommand<IEnumerable<User>, Task> LoadUserDataCommand => _loadUserDataCommand ??= new LoadUserListDataCommand(this, _userListVmProvider);

    private ReactiveCommand<Unit, Task<IEnumerable<User>>>? _searchUserDataCommand;
    public ReactiveCommand<Unit, Task<IEnumerable<User>>> SearchUserDataCommand => _searchUserDataCommand ??= new SearchUserDataCommand(this, _userListVmProvider);

    private ReactiveCommand<Unit, Task>? _saveUserDataCommand;
    public ReactiveCommand<Unit, Task> SaveUserDataCommand => _saveUserDataCommand ??= new SaveUserDataCommand(this, _userListVmProvider);

    private ReactiveCommand<Unit, Task>? _deleteUserDataCommand;
    public ReactiveCommand<Unit, Task> DeleteUserDataCommand => _deleteUserDataCommand ??= new DeleteUserDataCommand(this, _userListVmProvider);
    
    // Constructor 
    
    public UserListViewModel(IUserListVmProvider? userListVmProvider)
    {
        _userListVmProvider = userListVmProvider!;
        
        LoadUserDataCommand.Execute();
        SearchUserDataCommand.Subscribe(filteredUsers => LoadUserDataCommand.Execute(filteredUsers.Result));
        
        ConnectionDbState.CheckConnectionState.
            Subscribe(isConnected => DatabaseHasConnected = isConnected.Result);
    }
}
