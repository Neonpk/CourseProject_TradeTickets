using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.AirlineProvider;
using CourseProject_SellingTickets.Services.DiscountProvider;
using CourseProject_SellingTickets.Services.FlightClassProvider;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.Services.PhotoProvider;
using CourseProject_SellingTickets.Services.PlaceProvider;
using CourseProject_SellingTickets.Services.TicketProvider;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.Extensions.Hosting;
using Splat;

namespace CourseProject_SellingTickets.HostBuilders;

public static class AddViewModelsProvidersHostBuilderExtensions
{
    public static IHostBuilder AddViewModelsProviders(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            var resolver = Locator.CurrentMutable;
            var service = Locator.Current;
            
            // Included services 
            var iFlightDbProvider = service.GetService<IFlightDbProvider>();
            var iAircraftDbProvider = service.GetService<IAircraftDbProvider>();
            var iAirlineDbProvider = service.GetService<IAirlineDbProvider>();
            var iPlaceDbProvider = service.GetService<IPlaceDbProvider>();
            var iTicketDbProvider = service.GetService<ITicketDbProvider>();
            var iDiscountDbProvider = service.GetService<IDiscountDbProvider>();
            var iFlightClassDbProvider = service.GetService<IFlightClassDbProvider>();
            var iPhotoDbProvider = service.GetService<IPhotoDbProvider>();
            
            //ViewModels
            
            //ViewModels => AdminMode
            
            resolver.RegisterLazySingleton<IAircraftVmProvider>( () => 
                new AircraftVmProvider( iAircraftDbProvider, iPhotoDbProvider ) );
            
            resolver.RegisterLazySingleton<IPlaceVmProvider>( () => 
                new PlaceVmProvider( iPlaceDbProvider, iPhotoDbProvider ) );
            
            resolver.RegisterLazySingleton<IDiscountVmProvider>( () => 
                new DiscountVmProvider( iDiscountDbProvider ));
            
            resolver.RegisterLazySingleton<IPhotoVmProvider>(() => 
                new PhotoVmProvider( iPhotoDbProvider ));
            
            resolver.RegisterLazySingleton<IAirlineVmProvider>( () => 
                new AirlineVmProvider( iAirlineDbProvider ) );
            
            resolver.RegisterLazySingleton<IFlightClassVmProvider>( () =>
                new FlightClassVmProvider( iFlightClassDbProvider ) );
            
            //ViewModels => DispatcherMode
            
            resolver.RegisterLazySingleton<IFlightVmProvider>(() => 
                new FlightVmVmProvider(iFlightDbProvider, iAircraftDbProvider, iAirlineDbProvider, iPlaceDbProvider));
            
            resolver.RegisterLazySingleton<ITicketVmProvider>( () =>
                new TicketVmProvider(iTicketDbProvider, iDiscountDbProvider, iFlightClassDbProvider, iFlightDbProvider) );
            
        });

        return hostBuilder;
    }
}