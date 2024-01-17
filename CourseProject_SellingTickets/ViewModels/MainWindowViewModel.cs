using System;
using System.ComponentModel;
using System.Reactive;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CourseProject_SellingTickets.Helpers;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;
public class MainWindowViewModel : ViewModel
{

    // Services
    
    private INavigationService _navigationService;
    public INavigationService NavigationService { get { return _navigationService; } set { _navigationService = value; OnPropertyChanged(nameof(NavigationService)); } }
    
    public MainWindowViewModel(INavigationService navService)
    {
        NavigationService = navService;
        NavigationService.NavigateTo<AuthUserViewModel>();
    }
}