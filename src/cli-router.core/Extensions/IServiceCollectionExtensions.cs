using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CliRouter.Core.Routes;
using System.Reflection;
using CliRouter.Core.Models;
using CliRouter.Core.Checkers;
using CliRouter.Core.Finders;
using CliRouter.Core.Factories;

namespace CliRouter.Core.Extensions;

public static class IServiceCollectionExtensions
{
    //Use this one in the real world
    public static IServiceCollection AddCliRouting(this IServiceCollection services)
    {
        return services
            .AddTransient<Router>()
            .AddRoutes()
            .AddFactories()
        ;
    }

    //Use this one for Xunit tests for making sure it's pulling from our test project, not Xunit's assembly
    public static IServiceCollection AddCliRouting(this IServiceCollection services, Assembly assembly)
    {
        return services
            .AddTransient<Router>()
            .AddRoutes(assembly)
            .AddFactories()
        ;
    }

    private static IServiceCollection AddRoutes(this IServiceCollection services, params Assembly[] assembly)
        => services.AddImplementationsOf<ITemplatedRoute>(assembly);

    private static IServiceCollection AddImplementationsOf<T>(this IServiceCollection services, params Assembly[] assemblies)
    {
        var allTypes = TypeFinder.GetAllTypes(assemblies);

        var interfaceType = typeof(ITemplatedRoute);

        var implementations = allTypes
            .Where(TypeChecker.IsImplementationOf<T>);

        foreach (var implementation in implementations)
        {
            services.AddSingleton(interfaceType, implementation);
        }

        return services;
    }

    private static IServiceCollection AddFactories(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDynamicFactory, DynamicFactory>()
        ;
    }
}
