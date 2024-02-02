using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.AirlineProvider;
using CourseProject_SellingTickets.Services.PlaceProvider;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
using CourseProject_SellingTickets.ViewModels;
using Microsoft.Extensions.Hosting;
using Splat;

namespace CourseProject_SellingTickets.HostBuilders;

public static class AddDatabaseProvidersHostBuilderExtensions
{
    public static IHostBuilder AddDatabaseProviders(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            var resolver = Locator.CurrentMutable;
            var service = Locator.Current;
            
            //DB Providers

            var tradeTicketsDbContext = service.GetService<ITradeTicketsDbContextFactory>();

            resolver.RegisterLazySingleton<IConnectionStateProvider>(() =>
                new ConnectionStateProvider(tradeTicketsDbContext));
            
            resolver.RegisterLazySingleton<IFlightDbProvider>(() => 
                new DatabaseFlightDbProvider( tradeTicketsDbContext ));
            
            resolver.RegisterLazySingleton<IAircraftDbProvider>(() => 
                new DatabaseAircraftDbProvider( tradeTicketsDbContext ));
            
            resolver.RegisterLazySingleton<IAirlineDbProvider>(() => 
                new DatabaseAirlineDbProvider( tradeTicketsDbContext ));
            
            resolver.RegisterLazySingleton<IPlaceDbProvider>(() => 
                new DatabasePlaceDbProvider( tradeTicketsDbContext ));

        });

        return hostBuilder;
    }
}