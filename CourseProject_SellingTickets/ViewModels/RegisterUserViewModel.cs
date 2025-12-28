using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Commands.AuthCommands;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.UserProvider;
using CourseProject_SellingTickets.ValidationRules;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace CourseProject_SellingTickets.ViewModels;

public class RegisterUserViewModel : ViewModelBase, IValidatableViewModel
{
    
    // Services 
    
    private INavigationService? _navigationService;
    public INavigationService? NavigationService { get => _navigationService; set => this.RaiseAndSetIfChanged(ref _navigationService, value); }
    
    private IAuthProvider? _authProvider;
    public IAuthProvider? AuthProvider { get => _authProvider; set => this.RaiseAndSetIfChanged(ref _authProvider, value); }
    
    // Validations

    private string _errorValidations;
    public string ErrorValidations { get => _errorValidations; set => this.RaiseAndSetIfChanged(ref _errorValidations, value); }
    
    private string? _errorMessage;
    public string ErrorMessage { get => _errorMessage!; set { this.RaiseAndSetIfChanged(ref _errorMessage, value); this.RaisePropertyChanged(nameof(HasErrorMessage)); } }
    public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    
    public ValidationContext ValidationContext { get; } = new ValidationContext();
    
    // Page properties 
    
    private bool _isLoading;
    public bool IsLoading { get => _isLoading; set => this.RaiseAndSetIfChanged(ref _isLoading, value); }

    private string _login = String.Empty;
    public string Login { get => _login; set => this.RaiseAndSetIfChanged(ref _login, value); }

    private string _name = String.Empty; 
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }

    private string _password = String.Empty;
    public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }

    private string _confirmPassword = String.Empty;
    public string ConfirmPassword { get => _confirmPassword; set => this.RaiseAndSetIfChanged(ref _confirmPassword, value); }

    private bool _databaseHasConnected;
    public bool DatabaseHasConnected { get => _databaseHasConnected; set => this.RaiseAndSetIfChanged(ref _databaseHasConnected, value); }
    
    // Commands 
    
    private ReactiveCommand<Unit, Task>? _registerCommand;
    public ReactiveCommand<Unit, Task>? RegisterCommand => _registerCommand ??= new RegisterUserCommand(this, _authProvider!);
    
    private ReactiveCommand<Unit, Task>? _navigateBackCommand;
    public ReactiveCommand<Unit, Task>? NavigateBackCommand =>
        _navigateBackCommand ??= 
            ReactiveCommand.CreateFromObservable<Unit, Task>(_ => 
                Observable.Start(async () => NavigationService!.NavigateTo<AuthUserViewModel>())
            );
    
    // Constructor
    
    public RegisterUserViewModel(INavigationService navigationService, IAuthProvider authProvider)
    {
        NavigationService = navigationService;
        AuthProvider = authProvider;
        
        // Validation
        this.InitializeValidationRules();
        
        // Db connection state
        ConnectionDbState.CheckConnectionState.Subscribe(isConnected => DatabaseHasConnected = isConnected.Result);
    }
}
