using System.Windows.Input;
using CourseProject_SellingTickets.Services;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class AdminUserViewModel : ViewModelBase
{
    // Services 
    
    private INavigationService _navigationService;
    public INavigationService NavigationService { get =>  _navigationService; set { _navigationService = value; OnPropertyChanged(nameof(NavigationService)); } }
    
    // Commands (Event handlers)
    
    #pragma  warning disable
    private ICommand _exitCommand;
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
    
    public AdminUserViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }
}