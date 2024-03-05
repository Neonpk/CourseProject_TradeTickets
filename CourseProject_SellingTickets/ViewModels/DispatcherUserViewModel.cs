using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class DispatcherUserViewModel : ViewModelBase
{
    
    // Observable properties 
    
    private bool _showedSideBar = true;
    public bool ShowedSideBar { get => _showedSideBar;
        set { this.RaiseAndSetIfChanged(ref _showedSideBar, value); }
    }
    
    // Services 
    
    private INavigationService? _navigationMainService;
    public INavigationService? NavigationMainService { get => _navigationMainService;
        set { this.RaiseAndSetIfChanged(ref _navigationMainService, value); }
    }

    private INavigationService? _navigationDispatcherService;

    public INavigationService? NavigationDispatcherService
    {
        get => _navigationDispatcherService;
        set { this.RaiseAndSetIfChanged(ref _navigationDispatcherService, value); }
    }

    // Commands (Event handlers)
    
    #pragma warning disable
    private ICommand? _showSideBarCommand;
    public ICommand ShowSideBarCommand { get => _showSideBarCommand ??= ReactiveCommand.Create<Unit>((_) => ShowedSideBar = !ShowedSideBar); }
    
    #pragma  warning disable
    private ICommand? _exitCommand;
    public ICommand ExitCommand { get => _exitCommand ??= ReactiveCommand.Create<string>((obj) => NavigationMainService?.NavigateTo<AuthUserViewModel>()); }

    #pragma  warning disable
    private ICommand? _switchControlCommand;
    public ICommand SwitchControlCommand
    {
        get
        {
            return _switchControlCommand ??= ReactiveCommand.Create<string>((obj) =>
            {
                switch (obj)
                {
                    case "flights":
                        NavigationDispatcherService?.NavigateTo<FlightUserViewModel>();
                        break;
                    
                    case "tickets":
                        NavigationDispatcherService?.NavigateTo<TicketUserViewModel>();
                        break;
                }
            });
        } 
    }
    
    public DispatcherUserViewModel(INavigationService? navMainService, INavigationService? navDispatcherService)
    {
        NavigationMainService = navMainService;
        NavigationDispatcherService = navDispatcherService;
    }
}