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
            .AddImplementationsOfFromEntryAssembly<IRootRoutelet>()
            .AddRoutes()
        ;
    }

    //This one finds them all and registers them under the given interface
    private static IServiceCollection AddImplementationsOf<T>(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        Console.WriteLine($"Assembly: {assembly}");

        return services.AddImplementationsOf<T>(assembly);
    }

    private static IServiceCollection AddImplementationsOfFromEntryAssembly<T>(this IServiceCollection services)
    {
        var assembly = Assembly.GetEntryAssembly();

        return services.AddImplementationsOf<T>(assembly);
    }

    private static IServiceCollection AddImplementationsOf<T>(this IServiceCollection services, Assembly assembly)
    {
        var interfaceType = typeof(T);

        var implementedTypes = assembly.GetTypes()
            .Where(t => interfaceType.IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false });

        foreach (var implementedType in implementedTypes)
        {
            services.AddTransient(interfaceType, implementedType);
        }

        return services;
    }
}
