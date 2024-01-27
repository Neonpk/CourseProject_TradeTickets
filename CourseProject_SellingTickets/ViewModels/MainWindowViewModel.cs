using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CourseProject_SellingTickets.Helpers;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    
    // Services

    private INavigationService? _navigationService;
    public INavigationService? NavigationService { get { return _navigationService; } set { _navigationService = value; OnPropertyChanged(nameof(NavigationService)); } }
    
    // Observable properties 

    private bool _databaseHasConnected;
    public bool DatabaseHasConnected { get => _databaseHasConnected; set { _databaseHasConnected = value; OnPropertyChanged(nameof(DatabaseHasConnected)); } }
    
    // Commands 
    
    public MainWindowViewModel(INavigationService? navService)
    {
        NavigationService = navService;
        NavigationService?.NavigateTo<AuthUserViewModel>();
    }
}