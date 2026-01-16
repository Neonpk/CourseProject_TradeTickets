using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CourseProject_SellingTickets.Bootstrappers;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Interfaces;
using CourseProject_SellingTickets.Interfaces.Factories;
using CourseProject_SellingTickets.ViewModels;
using CourseProject_SellingTickets.Views;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.FileService;
using Microsoft.Extensions.Configuration;
using Splat;

namespace CourseProject_SellingTickets;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Locator.CurrentMutable.ConfigureServices((service,resolver) =>
        {

            // Enable Legacy TimestampBehavior for PostgreSQL

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

            // Integrate custom config 

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            
            resolver.RegisterLazySingleton(() => config);
            
            // DbContext Service 
            
            resolver.RegisterLazySingleton<ITradeTicketsDbContextFactory>(() =>
                new TradeTicketsDbContextFactory(config.GetConnectionString("Default")));

            // Navigation Services 

            resolver.RegisterLazySingleton<Func<Type, ViewModelBase?>>(
                () => viewModelType => (ViewModelBase?)service.GetService(viewModelType));

            var funcFactory = service.GetService<Func<Type, ViewModelBase>>();

            resolver.RegisterLazySingleton<INavigationService>(() =>
                new NavigationService(funcFactory), "mainNavigation");

            resolver.RegisterLazySingleton<INavigationService>(() =>
                new NavigationService(funcFactory), "dispatcherNavigation");

            resolver.RegisterLazySingleton<INavigationService>(() =>
                new NavigationService(funcFactory), "administratorNavigation");
            
            resolver.RegisterLazySingleton<INavigationService>(() => 
                new NavigationService(funcFactory), "clientUserNavigation");

        })
        .AddLibraries()
        .AddDatabaseProviders()
        .AddViewModelsProviders()
        .AddViewModels();
        
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
