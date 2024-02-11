using System.Reactive;
using System.Windows.Input;
using CourseProject_SellingTickets.Services;
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
    
    public AdminUserViewModel(INavigationService? navigationService)
    {
        NavigationService = navigationService;
    }
}