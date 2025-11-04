using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CliRouter.Core.Routes;
using System.Reflection;
using CliRouter.Core.Models;
using CliRouter.Core.Checkers;
using CliRouter.Core.Finders;

namespace CliRouter.Core.Extensions;

public static class IServiceCollectionExtensions
{
    //Use this one in the real world
    public static IServiceCollection AddCliRouting(this IServiceCollection services)
    {
        return services
            .AddTransient<Router>()
            .AddRoutes()
        ;
    }

    //Use this one for Xunit tests for making sure it's pulling from our test project, not Xunit's assembly
    public static IServiceCollection AddCliRouting(this IServiceCollection services, Assembly assembly)
    {
        return services
            .AddTransient<Router>()
            .AddRoutes(assembly)
        ;
    }

    private static IServiceCollection AddRoutes(this IServiceCollection services, params Assembly[] assembly)
    {
        var allTypes = TypeFinder.GetAllTypes(assembly);

        var interfaceType = typeof(ITemplatedRoute);

        var implementations = allTypes
            .Where(TypeChecker.IsImplementationOf<ITemplatedRoute>);

        foreach (var implementation in implementations)
        {
            services.AddTransient(interfaceType, implementation);
        }

        return services;
    }
}
