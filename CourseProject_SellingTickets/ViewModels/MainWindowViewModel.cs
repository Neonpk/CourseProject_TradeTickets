using System;
using CourseProject_SellingTickets.Interfaces;
using CourseProject_SellingTickets.Models;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    
    // Services
    private INavigationService? _navigationService;

    public INavigationService? NavigationService { get => _navigationService; set => this.RaiseAndSetIfChanged(ref _navigationService, value); }

    // Observable properties 

    private bool _databaseHasConnected;
    public bool DatabaseHasConnected { get => _databaseHasConnected; set => this.RaiseAndSetIfChanged(ref _databaseHasConnected, value); }

    // Commands 

    // Constructor 

    public MainWindowViewModel(INavigationService? navService)
    {
        ConnectionDbState.CheckConnectionState.Execute().Subscribe();
        ConnectionDbState.CheckConnectionState.Subscribe(isConnected => DatabaseHasConnected = isConnected.Result);
        
        NavigationService = navService;
        NavigationService?.NavigateTo<AuthUserViewModel>();
    }
}
