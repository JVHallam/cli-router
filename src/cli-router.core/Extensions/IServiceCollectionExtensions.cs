using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CliRouter.Core.Routes;
using System.Reflection;

namespace CliRouter.Core.Extensions;

public record TypeRelationship 
{
    public Type Parent { get; set; }
    public Type Child { get; set; }
}

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

    //TODO: Unit testable helper method
    private static bool IsSubInterfaceOf<T>(Type type)
    {
        if(!type.IsInterface)
        {
            return false;
        }

        //Is it a sub type
        if(!typeof(T).IsAssignableFrom(type))
        {
            return false;
        }

        //If it's not itself
        if(type == typeof(T))
        {
            return false;
        }

        return true;
    }

    //TODO: Unit testable helper method
    private static bool IsImplementationOf(Type parentType, Type childType)
    {
        if(!childType.IsClass)
        {
            return false;
        }
    
        if(childType.IsAbstract)
        {
            return false;
        }

        if(!parentType.IsAssignableFrom(childType))
        {
            return false;
        }

        return true;
    }

    //TODO: Unit testable helper method - more easily unit testable if you pass in the result of Assembly.GetTypes();
    private static IEnumerable<TypeRelationship> GetConcreteImplementationsOf(Type parentType)
    {
        System.Console.WriteLine($"Sub Route: {parentType}");

        var assembly = Assembly.GetEntryAssembly();

        var assemblyTypes = assembly.GetTypes();

        var implementations = 
            assemblyTypes
            .Where(t => IsImplementationOf(parentType, t));

        var relationships = implementations
            .Select(t => new TypeRelationship {
                Parent = parentType,
                Child = t
            });

        return relationships;
    }

    // Find all interfaces that inherit IRoute (excluding IRoute itself)
    // Then register all concrete classes that implement that sub class
    private static IServiceCollection AddRoutes(this IServiceCollection services)
    {
        var assembly = Assembly.GetEntryAssembly();

        var assemblyTypes = assembly.GetTypes();

        var subRouteImplementationRelationships = assemblyTypes
            .Where(IsSubInterfaceOf<IRoute>)
            .SelectMany(GetConcreteImplementationsOf)
            .ToList();

        //Register child types under their parent interface
        foreach(var relationship in subRouteImplementationRelationships)
        {
            services.AddTransient(relationship.Parent, relationship.Child);
        }

        return services;
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
