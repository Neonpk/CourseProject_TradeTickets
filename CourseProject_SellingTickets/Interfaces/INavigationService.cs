using CourseProject_SellingTickets.ViewModels;

namespace CourseProject_SellingTickets.Interfaces;

public interface INavigationService
{
    ViewModelBase? CurrentView { get; }
    void NavigateTo<T>(object? paramater = null) where T : ViewModelBase;
}
