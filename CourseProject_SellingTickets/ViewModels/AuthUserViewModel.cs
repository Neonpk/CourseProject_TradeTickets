using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Commands.AuthCommands;
using CourseProject_SellingTickets.Interfaces;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class AuthUserViewModel : ViewModelBase
{
    // Service

    private readonly IAuthProvider _authProvider;
    
    // Dynamic binding properties

    private AuthStates _authState;
    public AuthStates AuthState { get => _authState; set => this.RaiseAndSetIfChanged(ref _authState, value); }

    private bool _isLoading;
    public bool IsLoading { get => _isLoading; set => this.RaiseAndSetIfChanged(ref _isLoading, value); }

    public string Login { get; set; } = "";
    
    public string Password { get; set; } = "";
    
    // Services 
    
    private INavigationService? _navigationService;
    public INavigationService? NavigationService { get => _navigationService; set => this.RaiseAndSetIfChanged(ref _navigationService, value); }
    
    // Commands (Event handlers) 
    
    private ReactiveCommand<Unit, Task>? _loginCommand;
    public ReactiveCommand<Unit, Task> LoginCommand => _loginCommand ??= new LoginUserCommand(this, _authProvider);

    private ReactiveCommand<Unit, Task>? _registerCommand;
    public ReactiveCommand<Unit, Task>? RegisterCommand =>
        _registerCommand ??= 
            ReactiveCommand.CreateFromObservable<Unit, Task>(_ => 
                Observable.Start(() =>
                {
                    NavigationService!.NavigateTo<RegisterUserViewModel>();
                    return Task.CompletedTask;
                })
            );
    
    // Constructor

    public AuthUserViewModel(IAuthProvider? authProvider, INavigationService? navService)
    {
        _authProvider = authProvider!;
        NavigationService = navService;
    }
}
