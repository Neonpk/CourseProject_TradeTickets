using System;
using System.Reactive;
using System.Windows.Input;
using CourseProject_SellingTickets.Interfaces;
using CourseProject_SellingTickets.Models;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class ClientUserViewModel : ViewModelBase, IParameterReceiver
{
    // Custom parameters for viewModel 
    
    public Int64 UserId { get; private set; }
    
    // Observable properties 
    
    private bool _showedSideBar = true;
    public bool ShowedSideBar { get => _showedSideBar; set => this.RaiseAndSetIfChanged(ref _showedSideBar, value); }
    
    // Services 
    private INavigationService? _navigationMainService;
    public INavigationService? NavigationMainService { get => _navigationMainService; set => this.RaiseAndSetIfChanged(ref _navigationMainService, value); }

    
    private INavigationService? _navigationClientUserService;
    public INavigationService? NavigationClientUserService { get => _navigationClientUserService; set => this.RaiseAndSetIfChanged(ref _navigationClientUserService, value);
    }

    // Commands (Event handlers)
    
    #pragma warning disable
    private ICommand? _showSideBarCommand;
    public ICommand ShowSideBarCommand { get => _showSideBarCommand ??= ReactiveCommand.Create<Unit>(_ => ShowedSideBar = !ShowedSideBar); }
    
    #pragma  warning disable
    private ICommand? _exitCommand;
    public ICommand ExitCommand { get => _exitCommand ??= ReactiveCommand.Create<string>(_ => NavigationMainService?.NavigateTo<AuthUserViewModel>()); }

    #pragma  warning disable
    private ICommand? _switchControlCommand;
    public ICommand SwitchControlCommand
    {
        get
        {
            return _switchControlCommand ??= ReactiveCommand.Create<string>(obj =>
            {
                switch (obj)
                {
                    case "clientBalance":
                        NavigationClientUserService.NavigateTo<ClientBalanceUserViewModel>(UserId);
                        break;
                    
                    case "myFlights":
                        NavigationClientUserService.NavigateTo<FlightUserViewModel>(UserId);
                        break;
                    
                    case "myTickets":
                        NavigationClientUserService.NavigateTo<TicketUserViewModel>(new TicketUserViewModelParam { UserId = UserId, Include = true });
                        break;
                    
                    case "availableTickets":
                        NavigationClientUserService.NavigateTo<TicketUserViewModel>(new TicketUserViewModelParam {  UserId = UserId, Include = false });
                        break;
                }
            });
        } 
    }
    
    public ClientUserViewModel(INavigationService? navMainService, INavigationService? navClientUserService)
    {
        NavigationMainService = navMainService;
        NavigationClientUserService = navClientUserService;
    }
    
    public void ReceieveParameter(object parameter)
    {
        UserId = (Int64)(parameter is Int64 ? parameter : -1);
        NavigationClientUserService.NavigateTo<ClientBalanceUserViewModel>(UserId);
    }
}
