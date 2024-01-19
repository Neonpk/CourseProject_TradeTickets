using System;
using CourseProject_SellingTickets.ViewModels;

namespace CourseProject_SellingTickets.Services;

public interface INavigationService
{
    ObservableObject CurrentView { get; }
    void NavigateTo<T>() where T : ObservableObject;

}

public class NavigationService : ObservableObject, INavigationService
{
    private readonly Func<Type, ViewModelBase> _viewModelFactory;
    
    private ObservableObject _currentView;
    public ObservableObject CurrentView
    {
        get => _currentView;
        private set
        {
            _currentView = value;
            OnPropertyChanged(nameof(CurrentView));
        }
    }

    public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : ObservableObject
    {
        ObservableObject viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
        CurrentView = viewModel;
    }
}