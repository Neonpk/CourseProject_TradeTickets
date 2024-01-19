using CourseProject_SellingTickets.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CourseProject_SellingTickets.ViewModels;
using CourseProject_SellingTickets.Views;
using ReactiveUI;
using Splat;

namespace CourseProject_SellingTickets.HostBuilders;

public static class AddViewModelsHostBuilderExtensions
{
    public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            var resolver = Locator.CurrentMutable;
            var service = Locator.Current;
            
            //ViewModels
        
            resolver.RegisterLazySingleton<AdminUserViewModel>( () => new AdminUserViewModel( Locator.Current.GetService<INavigationService>() ));
        
            resolver.RegisterLazySingleton<DispatcherUserViewModel>( () => new DispatcherUserViewModel( Locator.Current.GetService<INavigationService>()) );
        
            resolver.RegisterLazySingleton<AuthUserViewModel>(() => new AuthUserViewModel(Locator.Current.GetService<INavigationService>() ));
        
            resolver.RegisterLazySingleton<MainWindowViewModel>(() => new MainWindowViewModel(
                service.GetService<INavigationService>()
            ));
            
        });

        return hostBuilder;
    }
}