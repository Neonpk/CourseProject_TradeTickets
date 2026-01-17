using CourseProject_SellingTickets.Interfaces.AircraftProviderInterface;
using CourseProject_SellingTickets.Interfaces.AirlineProviderInterface;
using CourseProject_SellingTickets.Interfaces.DiscountProviderInterface;
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

public static class AddViewModelsProvidersBootstrapperExtensions
{
    public static IMutableDependencyResolver AddViewModelsProviders(this IMutableDependencyResolver serviceBuilder)
    {
        return serviceBuilder.ConfigureServices((service,resolver) =>
        {
            // Included services 
            var iFlightDbProvider = service.GetService<IFlightDbProvider>();
            var iAircraftDbProvider = service.GetService<IAircraftDbProvider>();
            var iAirlineDbProvider = service.GetService<IAirlineDbProvider>();
            var iPlaceDbProvider = service.GetService<IPlaceDbProvider>();
            var iTicketDbProvider = service.GetService<ITicketDbProvider>();
            var iDiscountDbProvider = service.GetService<IDiscountDbProvider>();
            var iFlightClassDbProvider = service.GetService<IFlightClassDbProvider>();
            var iPhotoDbProvider = service.GetService<IPhotoDbProvider>();
            var iUserDbProvider = service.GetService<IUserDbProvider>();
            
            // Libraries 
            var iPasswordService = service.GetService<IPasswordService>(); 
            
            //ViewModels
            
            //ViewModels => AdminMode
            
            resolver.RegisterLazySingleton<IUserListVmProvider>( () => 
                new UserListVmProvider(iUserDbProvider!, iDiscountDbProvider!, iPasswordService!) );
            
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
                new TicketVmProvider(
                    iTicketDbProvider, 
                    iDiscountDbProvider, 
                    iFlightClassDbProvider, 
                    iFlightDbProvider,
                    iUserDbProvider
                    ) );
            
        });
    }
}
