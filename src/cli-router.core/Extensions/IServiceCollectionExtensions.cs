using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CliRouter.Core.Routes;
using System.Reflection;
using CliRouter.Core.Models;

namespace CliRouter.Core.Extensions;

//TODO: Once integration tests are setup, clean this up. It's messy.
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

    //Use this one for Xunit tests for making sure it's pulling from our project, not Xunit's assembly
    public static IServiceCollection AddCliRouting(this IServiceCollection services, Assembly assembly)
    {
        return services
            .AddTransient<Router>()
            .AddRoutesNew(assembly)
        ;
    }

    private static IServiceCollection AddRoutesNew(this IServiceCollection services, Assembly assembly)
    {
        var allTypes = GetAllTypes(assembly);

        var interfaceType = typeof(ITemplatedRoute);

        /*
        Console.WriteLine("Types:");
        foreach(var type in allTypes)
        {
            Console.WriteLine(type);
        }
        */

        //TODO: ChatGPT stink
        var implementations = allTypes
            .Where(t => interfaceType.IsAssignableFrom(t)
                        && !t.IsInterface
                        && !t.IsAbstract
                        && t.IsClass
                );

        Console.WriteLine("Implementations of ITemplatedRoute:");
        foreach (var implementation in implementations)
        {
            Console.WriteLine($"{implementation}");
            services.AddTransient(interfaceType, implementation);
        }

        return services;
    }






    private static IServiceCollection AddRoutes(this IServiceCollection services, Assembly assembly)
    {
        var allTypes = GetAllTypes(assembly);

        return services.RegisterTypes(allTypes);
    }

    private static IServiceCollection AddRoutes(this IServiceCollection services)
    {
        var allTypes = GetAllTypes();

        return services.RegisterTypes(allTypes);
    }

    // Find all interfaces that inherit IRoute (excluding IRoute itself)
    // Then register all concrete classes that implement that sub class
    private static IServiceCollection RegisterTypes(this IServiceCollection services, IEnumerable<Type> allTypes)
    {
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
    private static IEnumerable<Type> GetAllTypes(params Assembly[] assemblies)
    {
        var allAssemblies = new List<Assembly>()
        {
            Assembly.GetExecutingAssembly()
        };

        var entryAssembly = Assembly.GetEntryAssembly();

        if(entryAssembly != null)
        {
            allAssemblies.Add(entryAssembly);
        }

        allAssemblies = allAssemblies.Concat(assemblies).ToList();

        //TODO: Change this to a select statement
        var allTypes = new List<Type>();

        foreach(var assembly in allAssemblies)
        {
            var assemblyTypes = assembly.GetTypes();

            allTypes = allTypes.Concat(assemblyTypes).ToList();
        }

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

    private static bool IsImplementationOf<T>(Type childType)
    {
        var parentType = typeof(T);

        return IsImplementationOf(parentType, childType);
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
