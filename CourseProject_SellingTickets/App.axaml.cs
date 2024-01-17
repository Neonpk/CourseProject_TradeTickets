using System;
using System.ComponentModel.Design;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CourseProject_SellingTickets.ViewModels;
using CourseProject_SellingTickets.Views;
using CourseProject_SellingTickets.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
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

        // Navigation Service 
        
        Locator.CurrentMutable.RegisterLazySingleton<Func<Type, ViewModel>>(
            () => viewModelType => (ViewModel)Locator.Current.GetService(viewModelType));

        Locator.CurrentMutable.RegisterLazySingleton<INavigationService>(() => new NavigationService(Locator.Current.GetService<Func<Type, ViewModel>>()));
        
        //Navigation ViewModels
        
        Locator.CurrentMutable.RegisterLazySingleton<AdminUserViewModel>( () => new AdminUserViewModel( Locator.Current.GetService<INavigationService>() ));
        
        Locator.CurrentMutable.RegisterLazySingleton<DispatcherUserViewModel>( () => new DispatcherUserViewModel( Locator.Current.GetService<INavigationService>() ) );
        
        Locator.CurrentMutable.RegisterLazySingleton<AuthUserViewModel>(() => new AuthUserViewModel(Locator.Current.GetService<INavigationService>()));
        
        Locator.CurrentMutable.RegisterLazySingleton<MainWindowViewModel>(() => new MainWindowViewModel(Locator.Current.GetService<INavigationService>()));
        
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