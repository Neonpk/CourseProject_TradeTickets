using System;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Services;

public interface INavigationService
{
    ViewModelBase? CurrentView { get; }
    void NavigateTo<T>() where T : ViewModelBase;

}

public class NavigationService : ViewModelBase, INavigationService
{
    private readonly Func<Type, ViewModelBase?>? _viewModelFactory;
    
    private ViewModelBase? _currentView;
    public ViewModelBase? CurrentView
    {
        get => _currentView;
        private set
        {
            this.RaiseAndSetIfChanged(ref _currentView, value);
        }
    }

    public NavigationService(Func<Type, ViewModelBase?>? viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        ViewModelBase? viewModel = _viewModelFactory?.Invoke(typeof(TViewModel));
        CurrentView = viewModel;
    }
}