using System;
using CourseProject_SellingTickets.Interfaces;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Services;

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

    public void NavigateTo<TViewModel>(object? paramater = null) where TViewModel : ViewModelBase
    {
        ViewModelBase? viewModel = _viewModelFactory?.Invoke(typeof(TViewModel));

        if (viewModel is IParameterReceiver receiver)
        {
            receiver.ReceieveParameter(paramater ?? -1);
        }
        
        CurrentView = viewModel;
    }
}
