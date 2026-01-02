using System.Reactive;
using System.Windows.Input;
using CourseProject_SellingTickets.Interfaces;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class AdminUserViewModel : ViewModelBase
{
    // Observable properties 
    
    private bool _showedSideBar = true;
    public bool ShowedSideBar { get => _showedSideBar; set => this.RaiseAndSetIfChanged(ref _showedSideBar, value); }
    
    // Services 
    
    private INavigationService? _navigationService;
    public INavigationService? NavigationService { get =>  _navigationService; set => this.RaiseAndSetIfChanged(ref _navigationService, value); }

    private INavigationService? _navigationAdminService;
    public INavigationService? NavigationAdminService { get => _navigationAdminService; set => this.RaiseAndSetIfChanged(ref _navigationAdminService, value); }
    
    // Commands (Event handlers)
    
    #pragma warning disable
    private ICommand? _showSideBarCommand;

    public ICommand ShowSideBarCommand
    {
        get
        {
            return _showSideBarCommand ??= ReactiveCommand.Create<Unit>((_) =>
            {
                ShowedSideBar = !ShowedSideBar;
            });
        }
    }
    
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
                    case "aircraft":
                        NavigationAdminService?.NavigateTo<AircraftUserViewModel>();
                        break;
                    
                    case "place":
                        NavigationAdminService?.NavigateTo<PlaceUserViewModel>();
                        break;
                    
                    case "discount":
                        NavigationAdminService?.NavigateTo<DiscountUserViewModel>();
                        break;
                    
                    case "airline":
                        NavigationAdminService?.NavigateTo<AirlineUserViewModel>();
                        break;
                    
                    case "flight_class":
                        NavigationAdminService?.NavigateTo<FlightClassUserViewModel>();
                        break;
                    
                    case "photo":
                        NavigationAdminService?.NavigateTo<PhotoUserViewModel>();
                        break;
                }
            });
        } 
    }
    
    #pragma  warning disable
    private ICommand? _exitCommand;
    public ICommand ExitCommand
    {
        get
        {
            return _exitCommand ??= ReactiveCommand.Create<string>((obj) =>
            {
                NavigationService.NavigateTo<AuthUserViewModel>();
            });
        } 
    }
    
    public AdminUserViewModel(INavigationService? navigationService, INavigationService? navigationAdminService)
    {
        NavigationService = navigationService;
        NavigationAdminService = navigationAdminService;
    }
}