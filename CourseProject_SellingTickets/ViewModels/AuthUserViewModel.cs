using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.UserProvider;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class AuthUserViewModel : ViewModelBase
{
    // Service

    private readonly IAuthCheckerProvider _authCheckerProvider;
    
    // Dynamic variables

    private OperatingModes _operatingMode = OperatingModes.DispatcherMode;
    
    // Dynamic binding properties

    private AuthStates _authState;
    public AuthStates AuthState { get => _authState; set => this.RaiseAndSetIfChanged(ref _authState, value); }

    private bool _isLoading;
    public bool IsLoading { get => _isLoading; set => this.RaiseAndSetIfChanged(ref _isLoading, value); }
    
    public string Password { get; set; } = "";
    
    // Services 
    
    private INavigationService? _navigationService;
    public INavigationService? NavigationService { get => _navigationService; set => this.RaiseAndSetIfChanged(ref _navigationService, value); }
    
    // Constructor

    public AuthUserViewModel(IAuthCheckerProvider? authCheckerProvider, INavigationService? navService)
    {
        _authCheckerProvider = authCheckerProvider!;
        NavigationService = navService;
    }
    
    // Commands (Event handlers) 
    
    #pragma  warning disable
    private ICommand? _selectOperationModeCommand;
    public ICommand SelectOperationModeCommand
    {
        get
        {
            return _selectOperationModeCommand ??= ReactiveCommand.Create<string>((obj) =>
            {
                switch ( obj )
                {
                    case "administratorMode":
                        _operatingMode = OperatingModes.AdminMode;
                        break;
                    
                    case "dispatcherMode":
                        _operatingMode = OperatingModes.DispatcherMode;
                        break;
                }
            });
        } 
    }
    
    #pragma warning disable
    private ReactiveCommand<Unit, Task>? _loginCommand;
    public ReactiveCommand<Unit, Task> LoginCommand
    {
        get
        {
            return _loginCommand ??= ReactiveCommand.CreateFromObservable<Unit, Task>(_ =>
            {
                return Observable.Start(async () =>
                {
                    try
                    {
                        switch (_operatingMode)
                        {
                            case OperatingModes.AdminMode:

                                ConnectionDbState.CheckConnectionState.Execute().Subscribe();
                                
                                IsLoading = true;
                                AuthState = await _authCheckerProvider.CheckAdminPassword(Password);
                                IsLoading = false;

                                if (AuthState == AuthStates.Success)
                                    NavigationService.NavigateTo<AdminUserViewModel>();
                                break;

                            case OperatingModes.DispatcherMode:

                                ConnectionDbState.CheckConnectionState.Execute().Subscribe();
                                
                                IsLoading = true;
                                AuthState = await _authCheckerProvider.CheckDispatcherPassword(Password);
                                IsLoading = false;

                                if (AuthState == AuthStates.Success)
                                    NavigationService.NavigateTo<DispatcherUserViewModel>();
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        IsLoading = false;
                    }
                });
            });
        }
    }
}