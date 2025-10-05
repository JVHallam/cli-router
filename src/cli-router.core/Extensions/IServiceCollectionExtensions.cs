using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CliRouter.Core.Routes;
using System.Reflection;
using CliRouter.Core.Models;

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

    // Find all interfaces that inherit IRoute (excluding IRoute itself)
    // Then register all concrete classes that implement that sub class
    private static IServiceCollection AddRoutes(this IServiceCollection services)
    {
        var allTypes = GetAllTypes();

        var subRouteImplementationRelationships = allTypes
            .Where(IsSubInterfaceOf<IRoute>)
            .SelectMany(type => GetConcreteImplementationsOf(type, allTypes))
            .ToList();

        //Register child types under their parent interface
        foreach(var relationship in subRouteImplementationRelationships)
        {
            services.AddTransient(relationship.Parent, relationship.Child);
        }

        return services;
    }

    //TODO: Unit testable helper method
    private static IEnumerable<Type> GetAllTypes()
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        var executingAssemblyTypes = executingAssembly.GetTypes();

        var entryAssembly = Assembly.GetEntryAssembly();

        if(entryAssembly == null)
        {
            return executingAssemblyTypes;
        }

        var entryAssemblyTypes = entryAssembly.GetTypes();
        var allTypes = entryAssemblyTypes.Concat(executingAssemblyTypes);

        return allTypes;
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
    private static IEnumerable<TypeRelationship> GetConcreteImplementationsOf(Type parentType, IEnumerable<Type> types)
    {
        var implementations = types
            .Where(t => IsImplementationOf(parentType, t));

        var relationships = implementations
            .Select(t => new TypeRelationship {
                Parent = parentType,
                Child = t
            });

        return relationships;
    }
}
