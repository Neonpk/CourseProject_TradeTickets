using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.AirlineProvider;
using CourseProject_SellingTickets.Services.DiscountProvider;
using CourseProject_SellingTickets.Services.FlightClassProvider;
using CourseProject_SellingTickets.Services.PlaceProvider;
using CourseProject_SellingTickets.Services.TicketProvider;
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
                new ConnectionStateProvider( tradeTicketsDbContext! ));
            
            resolver.RegisterLazySingleton<IFlightDbProvider>(() => 
                new FlightDbProvider( tradeTicketsDbContext! ));
            
            resolver.RegisterLazySingleton<ITicketDbProvider>( () => 
                new TicketDbProvider( tradeTicketsDbContext! ));
            
            resolver.RegisterLazySingleton<IAircraftDbProvider>(() => 
                new AircraftDbProvider( tradeTicketsDbContext! ));
            
            resolver.RegisterLazySingleton<IAirlineDbProvider>(() => 
                new AirlineDbProvider( tradeTicketsDbContext! ));
            
            resolver.RegisterLazySingleton<IPlaceDbProvider>(() => 
                new PlaceDbProvider( tradeTicketsDbContext! ));

            resolver.RegisterLazySingleton<IDiscountDbProvider>( () => 
                new DiscountDbProvider( tradeTicketsDbContext! ) );
            
            resolver.RegisterLazySingleton<IFlightClassDbProvider>( () => 
                new FlightClassDbProvider(tradeTicketsDbContext!) );
            
        });

        return hostBuilder;
    }
}