using CourseProject_SellingTickets.Interfaces.AircraftProviderInterface;
using CourseProject_SellingTickets.Interfaces.AirlineProviderInterface;
using CourseProject_SellingTickets.Interfaces.DiscountProviderInterface;
using CourseProject_SellingTickets.Interfaces.Factories;
using CourseProject_SellingTickets.Interfaces.FlightClassProviderInterface;
using CourseProject_SellingTickets.Interfaces.FlightProviderInterface;
using CourseProject_SellingTickets.Interfaces.PhotoProviderInterface;
using CourseProject_SellingTickets.Interfaces.PlaceProviderInterface;
using CourseProject_SellingTickets.Interfaces.TicketProviderInterface;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface.PasswordServiceInterface;
using CourseProject_SellingTickets.Services.AircraftProvider;
using CourseProject_SellingTickets.Services.AirlineProvider;
using CourseProject_SellingTickets.Services.DiscountProvider;
using CourseProject_SellingTickets.Services.FlightClassProvider;
using CourseProject_SellingTickets.Services.FlightProvider;
using CourseProject_SellingTickets.Services.PhotoProvider;
using CourseProject_SellingTickets.Services.PlaceProvider;
using CourseProject_SellingTickets.Services.TicketProvider;
using CourseProject_SellingTickets.Services.UserProvider;
using Splat;

namespace CourseProject_SellingTickets.Bootstrappers;

public static class AddDatabaseProvidersBootstrapperExtensions
{
    public static IMutableDependencyResolver AddDatabaseProviders(this IMutableDependencyResolver serviceBuilder)
    {
        return serviceBuilder.ConfigureServices((service, resolver) =>
        {

            //DB Providers

            var tradeTicketsDbContext = service.GetService<ITradeTicketsDbContextFactory>();

            // User

            resolver.RegisterLazySingleton<IUserDbProvider>(() =>
                new UserDbProvider(tradeTicketsDbContext!));

            resolver.RegisterLazySingleton<IAuthProvider>(() =>
                new AuthProvider(
                    service.GetService<IUserDbProvider>()!, 
                    service.GetService<IPasswordService>()!
                    )
            );

            // Other

            resolver.RegisterLazySingleton<IFlightDbProvider>(() =>
                new FlightDbProvider(tradeTicketsDbContext!));

            resolver.RegisterLazySingleton<ITicketDbProvider>(() =>
                new TicketDbProvider(tradeTicketsDbContext!));

            resolver.RegisterLazySingleton<IAircraftDbProvider>(() =>
                new AircraftDbProvider(tradeTicketsDbContext!));

            resolver.RegisterLazySingleton<IAirlineDbProvider>(() =>
                new AirlineDbProvider(tradeTicketsDbContext!));

            resolver.RegisterLazySingleton<IPlaceDbProvider>(() =>
                new PlaceDbProvider(tradeTicketsDbContext!));

            resolver.RegisterLazySingleton<IDiscountDbProvider>(() =>
                new DiscountDbProvider(tradeTicketsDbContext!));

            resolver.RegisterLazySingleton<IFlightClassDbProvider>(() =>
                new FlightClassDbProvider(tradeTicketsDbContext!));

            resolver.RegisterLazySingleton<IPhotoDbProvider>(() =>
                new PhotoDbProvider(tradeTicketsDbContext!));

        });
    }
}