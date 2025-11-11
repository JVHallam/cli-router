using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CliRouter.Core.Routes;
using System.Reflection;
using CliRouter.Core.Models;
using CliRouter.Core.Checkers;
using CliRouter.Core.Finders;
using CliRouter.Core.Factories;
using CliRouter.Core.Orchestrators;
using ObjectFactory = CliRouter.Core.Factories.ObjectFactory;
using System.Collections;
using System.Collections.Generic;
using CliRouter.Core.Services;

namespace CliRouter.Core.Extensions;

public static class IServiceCollectionExtensions
{
    //Use this one in the real world
    public static IServiceCollection AddCliRouting(this IServiceCollection services)
    {
        return services
            .AddSingleton<RouteOrchestrator>()
            .AddRoutes()
            .AddFactories()
            .AddServices()
        ;
    }

    //Use this one for Xunit tests for making sure it's pulling from our test project, not Xunit's assembly
    public static IServiceCollection AddCliRouting(this IServiceCollection services, Assembly assembly)
    {
        return services
            .AddSingleton<RouteOrchestrator>()
            .AddRoutes(assembly)
            .AddFactories()
            .AddServices()
        ;
    }

    private static IServiceCollection AddFactories(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDynamicFactory, DynamicFactory>()
            .AddSingleton<IFullyQualifiedRouteFactory, FullyQualifiedRouteFactory>()
            .AddSingleton<IGenericValueFactory, GenericValueFactory>()
            .AddSingleton<IObjectFactory, ObjectFactory>()
            .AddSingleton<IFlagValueFactory, FlagValueFactory>()
        ;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IGenericTypeService, GenericTypeService>()
            .AddSingleton<IObjectMapper, ObjectMapper>()
            .AddSingleton<IRouteKeyService, RouteKeyService>()
            .AddSingleton<IArgsService, ArgsService>()
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
}
