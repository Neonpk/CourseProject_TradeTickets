using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.AirlineProvider;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.Services.PlaceProvider;
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
            
            //ViewModels
            
            resolver.RegisterLazySingleton<IFlightVmProvider>(() => 
                new FlightVmVmProvider(iFlightDbProvider, iAircraftDbProvider, iAirlineDbProvider, iPlaceDbProvider));
            
        });

        return hostBuilder;
    }
}