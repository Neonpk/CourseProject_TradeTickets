using System;
using Splat;

namespace CourseProject_SellingTickets.HostBuilders;

public static class LocatorBootstrapperExtensions
{
    public static IMutableDependencyResolver ConfigureServices(this IMutableDependencyResolver resolver, Action<IReadonlyDependencyResolver, IMutableDependencyResolver> callback)
    {
        callback(Locator.Current, Locator.CurrentMutable);
        return resolver;
    }
}