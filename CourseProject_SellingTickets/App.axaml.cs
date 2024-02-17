using System;
using System.ComponentModel.Design;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
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
    private IHost? _host;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, hostService) => {
                
                // Enable Legacy TimestampBehavior for PostgreSQL
                
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
                
                // Avalonia Locators 
                
                var resolver = Locator.CurrentMutable;
                var service = Locator.Current;
                
                // Integrate Database (PostgreSQL) 

                /*
                resolver.RegisterLazySingleton<ITradeTicketsDbContextFactory>(() =>
                    new TradeTicketsDbContextFactory(hostContext.Configuration.GetConnectionString("Default")));
                */
                
                resolver.RegisterLazySingleton<ITradeTicketsDbContextFactory>(() => 
                    new TradeTicketsDbContextFactory("Server=127.0.0.1;Port=5432;Database=TradeTickets;User Id=postgres;Password=;CommandTimeout=20;"));
                
                // Navigation Services 
                
                resolver.RegisterLazySingleton<Func<Type, ViewModelBase?>>(
                    () => viewModelType => (ViewModelBase?)service.GetService(viewModelType));
                
                var funcFactory = service.GetService<Func<Type, ViewModelBase>>();
                
                resolver.RegisterLazySingleton<INavigationService>(() => 
                    new NavigationService( funcFactory ), "mainNavigation");

                resolver.RegisterLazySingleton<INavigationService>(() => 
                    new NavigationService( funcFactory ), "dispatcherNavigation");

                resolver.RegisterLazySingleton<INavigationService>(() =>
                    new NavigationService(funcFactory), "administratorNavigation");

            })
            .AddDatabaseProviders()
            .AddViewModelsProviders()
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