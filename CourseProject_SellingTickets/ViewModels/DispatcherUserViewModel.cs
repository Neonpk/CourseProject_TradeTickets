using CourseProject_SellingTickets.Services;

namespace CourseProject_SellingTickets.ViewModels;

public class DispatcherUserViewModel : ViewModel
{
    // Services 
    
    private INavigationService _navigationService;
    public INavigationService NavigationService { get { return _navigationService; } set { _navigationService = value; OnPropertyChanged(nameof(NavigationService)); } }
    
    public DispatcherUserViewModel(INavigationService navService)
    {
        NavigationService = navService;
    }
}