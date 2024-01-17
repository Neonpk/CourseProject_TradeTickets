using CourseProject_SellingTickets.Services;

namespace CourseProject_SellingTickets.ViewModels;

public class AdminUserViewModel : ViewModel
{
    // Services 
    
    private INavigationService _navigationService;
    public INavigationService NavigationService { get =>  _navigationService; set { _navigationService = value; OnPropertyChanged(nameof(NavigationService)); } }
    
    public AdminUserViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }
}