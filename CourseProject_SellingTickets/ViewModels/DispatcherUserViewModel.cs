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
    public bool ShowedSideBar { get => _showedSideBar; set { _showedSideBar = value; OnPropertyChanged(nameof(ShowedSideBar)); } }
    
    // Services 
    
    private INavigationService? _navigationMainService;
    public INavigationService? NavigationMainService { get => _navigationMainService; set { _navigationMainService = value; OnPropertyChanged(nameof(NavigationMainService)); } }

    private INavigationService? _navigationDispatcherService;
    public INavigationService? NavigationDispatcherService { get => _navigationDispatcherService; set { _navigationDispatcherService = value; OnPropertyChanged(nameof(NavigationDispatcherService)); } }
    
    // Commands (Event handlers)
    
    #pragma warning disable
    private ICommand? _showSideBarCommand;
    public ICommand ShowSideBarCommand { get => _showSideBarCommand ??= ReactiveCommand.Create<Unit>((_) => ShowedSideBar = !ShowedSideBar); }
    
    #pragma  warning disable
    private ICommand? _exitCommand;
    public ICommand ExitCommand { get => _exitCommand ??= ReactiveCommand.Create<string>((obj) => NavigationMainService?.NavigateTo<AuthUserViewModel>()); }

    #pragma  warning disable
    private ICommand? _fetchCommand;
    public ICommand FetchCommand
    {
        get
        {
            return _fetchCommand ??= ReactiveCommand.Create<string>((obj) =>
            {
                switch (obj)
                {
                    case "flights":
                        NavigationDispatcherService?.NavigateTo<FlightUserViewModel>();
                        break;
                    
                    case "tickets":
                        NavigationDispatcherService?.NavigateTo<ViewModelBase>();
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