using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
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
            
            // Included services 
            
            INavigationService? mainNavigation = service.GetService<INavigationService>("mainNavigation");
            INavigationService? dispatcherNavigation = service.GetService<INavigationService>("dispatcherNavigation");

            IFlightProvider? flightProvider = service.GetService<IFlightProvider>();
            
            //General ViewModels
                                
            resolver.RegisterLazySingleton<FlightUserViewModel>( () => new FlightUserViewModel( flightProvider ) );
            
            resolver.RegisterLazySingleton<AdminUserViewModel>( () => new AdminUserViewModel( mainNavigation ));
        
            resolver.RegisterLazySingleton<DispatcherUserViewModel>( () => new DispatcherUserViewModel( mainNavigation, dispatcherNavigation ) );
            
            resolver.RegisterLazySingleton<AuthUserViewModel>(() => new AuthUserViewModel( mainNavigation ));
        
            resolver.RegisterLazySingleton<MainWindowViewModel>(() => new MainWindowViewModel( mainNavigation ));
            
        });

        return hostBuilder;
    }
}