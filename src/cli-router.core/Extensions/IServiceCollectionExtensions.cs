using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CliRouter.Core.Routes;
using System.Reflection;

namespace CliRouter.Core.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCliRouting(this IServiceCollection services)
    {
        return services
            .AddTransient<RootRoute>()
            .AddRoutes()
        ;
    }

    private static IServiceCollection AddRoutes(this IServiceCollection services)
    {
        return services.AddImplementationsOf<IRoute>();
    }

    //This one finds them all and registers them under the interface's type, so T[] will be injected
    private static IServiceCollection AddImplementationsOfInterfaceAsInterface<T>(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var interfaceType = typeof(T);

        var types = assembly.GetTypes()
            .Where(t => interfaceType.IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false });

        foreach (var impl in types)
        {
            services.AddTransient(interfaceType, impl);
        }

        return services;
    }

    //This one finds them all and registers them as their own selves
    private static IServiceCollection AddImplementationsOf<T>(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var interfaceType = typeof(T);

        var types = assembly.GetTypes()
            .Where(t => interfaceType.IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false });

        foreach (var impl in types)
        {
            services.AddTransient(impl);
        }

        return services;
    }
}
