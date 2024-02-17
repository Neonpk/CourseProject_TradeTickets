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
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.Services.TicketProvider;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
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

    public MainWindowViewModel(INavigationService? navService, ITicketVmProvider ticketDbProvider)
    {

        /*
        ReactiveCommand.CreateFromObservable(() => Observable.Start(async () =>
        {
            return await ticketDbProvider.GetTopTickets(50);
        })).Execute().Subscribe(x =>
        {
            var item = x.Result;

        });
        */

        NavigationService = navService;
        NavigationService?.NavigateTo<AuthUserViewModel>();
        
    }
}