using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Services;
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
            
            resolver.RegisterLazySingleton<IFlightProvider>(() => 
                new DatabaseFlightProvider( service.GetService<ITradeTicketsDbContextFactory>() ));

        });

        return hostBuilder;
    }
}