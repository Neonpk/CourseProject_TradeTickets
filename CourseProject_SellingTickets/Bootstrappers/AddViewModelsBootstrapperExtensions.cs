using CourseProject_SellingTickets.Interfaces;
using CourseProject_SellingTickets.Interfaces.AircraftProviderInterface;
using CourseProject_SellingTickets.Interfaces.AirlineProviderInterface;
using CourseProject_SellingTickets.Interfaces.DiscountProviderInterface;
using CourseProject_SellingTickets.Interfaces.FlightClassProviderInterface;
using CourseProject_SellingTickets.Interfaces.FlightProviderInterface;
using CourseProject_SellingTickets.Interfaces.PhotoProviderInterface;
using CourseProject_SellingTickets.Interfaces.PlaceProviderInterface;
using CourseProject_SellingTickets.Interfaces.TicketProviderInterface;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.ViewModels;
using Splat;

namespace CourseProject_SellingTickets.Bootstrappers;

public static class AddViewModelsBootstrapperExtensions
{
    public static IMutableDependencyResolver AddViewModels(this IMutableDependencyResolver serviceBuilder)
    {
        return serviceBuilder.ConfigureServices((service, resolver) =>
        {
            // Included services 

            IUserDbProvider? userDbProvider = service.GetService<IUserDbProvider>();
            IAuthProvider? authProvider = service.GetService<IAuthProvider>();
            
            INavigationService? mainNavigation = service.GetService<INavigationService>("mainNavigation");
            INavigationService? dispatcherNavigation = service.GetService<INavigationService>("dispatcherNavigation");
            INavigationService? adminNavigation = service.GetService<INavigationService>("administratorNavigation");
            INavigationService? clientUserNavigation = service.GetService<INavigationService>("clientUserNavigation");

            //IConnectionStateProvider? connectionStateProvider = service.GetService<IConnectionStateProvider>();
            
            IAircraftVmProvider? aircraftProvider = service.GetService<IAircraftVmProvider>();
            IPlaceVmProvider? placeVmProvider = service.GetService<IPlaceVmProvider>();
            IDiscountVmProvider? discountVmProvider = service.GetService<IDiscountVmProvider>();
            IPhotoVmProvider? photoVmProvider = service.GetService<IPhotoVmProvider>();
            IFlightClassVmProvider? flightClassVmProvider = service.GetService<IFlightClassVmProvider>();
            IAirlineVmProvider? airlineVmProvider = service.GetService<IAirlineVmProvider>();
            
            IFlightVmProvider? flightProvider = service.GetService<IFlightVmProvider>();
            ITicketVmProvider? ticketProvider = service.GetService<ITicketVmProvider>();
            
            //General ViewModels
            
            // Admin 
            
            resolver.RegisterLazySingleton( () => new AircraftUserViewModel( aircraftProvider ) );
            
            resolver.RegisterLazySingleton( () => new PlaceUserViewModel( placeVmProvider ) );
            
            resolver.RegisterLazySingleton( () => new DiscountUserViewModel( discountVmProvider ) );
            
            resolver.RegisterLazySingleton( () => new PhotoUserViewModel( photoVmProvider ) );
            
            resolver.RegisterLazySingleton( () => new FlightClassUserViewModel( flightClassVmProvider ) );
            
            resolver.RegisterLazySingleton( () => new AirlineUserViewModel( airlineVmProvider ) );
            
            //--AdminUserViewModel
            
            resolver.RegisterLazySingleton( () => new AdminUserViewModel( mainNavigation, adminNavigation ));
            
            // Dispatcher 
            
            resolver.RegisterLazySingleton( () => new TicketUserViewModel( ticketProvider ) );
            
            resolver.RegisterLazySingleton( () => new FlightUserViewModel( flightProvider ) );
            
            //--DispatcherUserViewModel
            
            resolver.RegisterLazySingleton( () => new DispatcherUserViewModel( mainNavigation, dispatcherNavigation ) );
            
            // UserClient 
            
            resolver.RegisterLazySingleton(() => new ClientBalanceUserViewModel( userDbProvider! ));
            
            //--ClientUserViewModel
            
            resolver.RegisterLazySingleton( () => new ClientUserViewModel( mainNavigation, clientUserNavigation ) );
            
            // Main 
            
            resolver.RegisterLazySingleton(() => new AuthUserViewModel( authProvider, mainNavigation ));

            resolver.RegisterLazySingleton(() => new RegisterUserViewModel( mainNavigation!, authProvider! ));
        
            resolver.RegisterLazySingleton(() => new MainWindowViewModel( mainNavigation ));
            
        });
    }
}
