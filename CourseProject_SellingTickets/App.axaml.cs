using System;
using System.ComponentModel.Design;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.HostBuilders;
using CourseProject_SellingTickets.ViewModels;
using CourseProject_SellingTickets.Views;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Splat;

namespace CourseProject_SellingTickets;

public partial class App : Application
{
    private IServiceProvider _container;
    private IHost _host;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, hostService) => {
                    
                // Avalonia Locators 
                
                var resolver = Locator.CurrentMutable;
                var service = Locator.Current;
                
                // Integrate Database (PostgreSQL) 

                resolver.RegisterLazySingleton<ITradeTicketsDbContextFactory>(() =>
                    new TradeTicketsDbContextFactory(hostContext.Configuration.GetConnectionString("Default")));
                
                // Navigation Service 
        
                resolver.RegisterLazySingleton<Func<Type, ViewModelBase>>(
                    () => viewModelType => (ViewModelBase)Locator.Current.GetService(viewModelType));

                resolver.RegisterLazySingleton<INavigationService>(() => 
                    new NavigationService(Locator.Current.GetService<Func<Type, ViewModelBase>>()));
                
            })
            .AddViewModels()
            .Build();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Locator.Current.GetService<MainWindowViewModel>()
            };
        }
        
        base.OnFrameworkInitializationCompleted();
    }
}