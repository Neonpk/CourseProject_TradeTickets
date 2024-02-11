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

public class AuthUserViewModel : ViewModelBase
{
    // Dynamic variables

    private OperatingModes _operatingMode = OperatingModes.DispatcherMode;
    
    // Dynamic binding properties

    private AuthStates _authState;
    public AuthStates AuthState { get => _authState; set => this.RaiseAndSetIfChanged(ref _authState, value); }

    public string Password { get; set; } = "";
    
    // Services 
    
    private INavigationService? _navigationService;
    public INavigationService? NavigationService { get => _navigationService; set => this.RaiseAndSetIfChanged(ref _navigationService, value); }
    
    // Constructor

    public AuthUserViewModel(INavigationService? navService)
    {
        NavigationService = navService;
    }
    
    // Commands (Event handlers) 
    
    #pragma  warning disable
    private ICommand? _selectOperationModeCommand;
    public ICommand SelectOperationModeCommand
    {
        get
        {
            return _selectOperationModeCommand ??= ReactiveCommand.Create<string>((obj) =>
            {
                switch ( obj )
                {
                    case "administratorMode":

                        _operatingMode = OperatingModes.AdminMode;
                        
                        break;
                    
                    case "dispatcherMode":

                        _operatingMode = OperatingModes.DispatcherMode;
                        
                        break;
                }
            });
        } 
    }
    
    #pragma warning disable
    private ICommand? _loginCommand;
    public ICommand LoginCommand
    {
        get
        {
            return _loginCommand ??= ReactiveCommand.Create<object>((obj) =>
            {

                switch ( _operatingMode )
                {
                    case OperatingModes.AdminMode:

                        AuthState = AuthChecker.CheckAdminPassword(Password);
                        
                        if ( AuthState == AuthStates.Success )
                            NavigationService.NavigateTo<AdminUserViewModel>();
                        
                        break;
                    
                    case OperatingModes.DispatcherMode:

                        AuthState = AuthChecker.CheckDispatcherPassword(Password);
                        
                        if ( AuthState == AuthStates.Success )
                            NavigationService.NavigateTo<DispatcherUserViewModel>();
                        
                        break;
                }
                
            });
        }
    }
}