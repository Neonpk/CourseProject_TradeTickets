using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class DispatcherUserViewModel : ViewModelBase
{
    // Services 
    
    private INavigationService _navigationService;
    public INavigationService NavigationService { get { return _navigationService; } set { _navigationService = value; OnPropertyChanged(nameof(NavigationService)); } }
    
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
    
    #pragma  warning disable
    private ICommand _fetchCommand;
    public ICommand FetchCommand
    {
        get
        {
            return _fetchCommand ??= ReactiveCommand.CreateFromObservable<Unit>(() =>
            {
                return Observable.Start(() =>
                {
                    
                });
            });
        } 
    }
    
    public DispatcherUserViewModel(INavigationService navService)
    {
        NavigationService = navService;
    }
}