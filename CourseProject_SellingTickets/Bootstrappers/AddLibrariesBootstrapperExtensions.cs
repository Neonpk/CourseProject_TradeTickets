using CourseProject_SellingTickets.Factories;
using CourseProject_SellingTickets.Interfaces.FileServiceInterface;
using CourseProject_SellingTickets.Interfaces.FreeImageServiceInterface;
using CourseProject_SellingTickets.Services.FileService;
using CourseProject_SellingTickets.Services.FreeImageService;
using Microsoft.Extensions.Configuration;
using Splat; 

namespace CourseProject_SellingTickets.Bootstrappers;

public static class AddLibrariesBootstrapperExtensions
{
    public static IMutableDependencyResolver AddLibraries(this IMutableDependencyResolver serviceBuilder)
    {
        return serviceBuilder.ConfigureServices((service, resolver) =>
        {
            IConfiguration? config = service.GetService<IConfiguration>();
            
            // Inject FreeImageService
            
            resolver.RegisterLazySingleton<IFreeImageService>(() => 
                new FreeImageService(
                    new HttpClientFactory(), 
                    config?["HostingServices:FreeImagehost:ApiKey"]!
                )
            );
            
            // Inject FileService 
            
            resolver.RegisterLazySingleton<IFileService>(() => new FileService());

        });
    }
}
