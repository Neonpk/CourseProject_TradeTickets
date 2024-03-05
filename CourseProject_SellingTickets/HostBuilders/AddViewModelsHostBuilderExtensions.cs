using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.Services.TicketProvider;
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
            INavigationService? adminNavigation = service.GetService<INavigationService>("administratorNavigation");

            IConnectionStateProvider? connectionStateProvider = service.GetService<IConnectionStateProvider>();

            IAircraftVmProvider? aircraftProvider = service.GetService<IAircraftVmProvider>();
            
            IFlightVmProvider? flightProvider = service.GetService<IFlightVmProvider>();
            ITicketVmProvider? ticketProvider = service.GetService<ITicketVmProvider>();
            
            //General ViewModels
            
            resolver.RegisterLazySingleton<AircraftUserViewModel>( () => new AircraftUserViewModel( aircraftProvider, connectionStateProvider ) );
            
            resolver.RegisterLazySingleton<AdminUserViewModel>( () => new AdminUserViewModel( mainNavigation, adminNavigation ));
            
            //
            
            resolver.RegisterLazySingleton<TicketUserViewModel>( () => new TicketUserViewModel( ticketProvider, connectionStateProvider ) );
            
            resolver.RegisterLazySingleton<FlightUserViewModel>( () => new FlightUserViewModel( flightProvider, connectionStateProvider ) );
            
            resolver.RegisterLazySingleton<DispatcherUserViewModel>( () => new DispatcherUserViewModel( mainNavigation, dispatcherNavigation ) );
            
            // 
            
            resolver.RegisterLazySingleton<AuthUserViewModel>(() => new AuthUserViewModel( mainNavigation ));
        
            resolver.RegisterLazySingleton<MainWindowViewModel>(() => new MainWindowViewModel( mainNavigation ));
            
        });

        return hostBuilder;
    }
}