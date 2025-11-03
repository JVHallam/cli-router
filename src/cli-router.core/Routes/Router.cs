using System.Collections.Generic;
using CliRouter.Core.Factories;

namespace CliRouter.Core.Routes;

public class Router
{
    private Dictionary<string, ITemplatedRoute> _routes;

    //TODO: Should a constructor be THIS complex?
    public Router(IEnumerable<ITemplatedRoute> routes)
    {
        //TODO: We want to use DI, NOT static classes
        //TODO: Also tolist when I can avoid it
        var fullyQualifiedRoutes = FullyQualifiedRouteFactory.Create(routes.ToList());

        _routes = ToDictionary(fullyQualifiedRoutes);
    }

    public async Task HandleAsync(string[] args)
    {
        var argsAsString = String.Join(" ", args);

        var deepestKey = GetDeepestKeyR(_routes, argsAsString);

        Console.WriteLine($"Key that was matched: {deepestKey}");

        Console.WriteLine("keys");
        foreach(var key in _routes.Keys)
        {
            Console.WriteLine(key);
        }

        if(deepestKey == null)
        {
            Console.WriteLine("No key was matched!");
            return;
        }

        var route = _routes[deepestKey]!;
        
        var deepestKeyLength = deepestKey.Split(" ").Length;

        //Remove that from the args
        var rightArgs = args
            .Skip(deepestKeyLength)
            .ToArray();

        //Now we need to convert that into an object, if the route requires it
        var implementationTypeArgument = GetImplementationType(route);

        Console.WriteLine($"Implementation Type Argument: {implementationTypeArgument}");

        var genericValues = GenericValueFactory.Create(implementationTypeArgument, rightArgs);

        var constructorValues = genericValues
            .Where(x => !x.IsFlag)
            .ToList();

        /*
        var flagValues = genericValues
            .Where(x => x.IsFlag)
            .ToList();
        */

        var argsForConstructor = ObjectFactory.Create(constructorValues);

        //var argsForFlags = ObjectFactory.Create(flagValues);

        var request = DynamicFactory.CreateInstance(implementationTypeArgument, argsForConstructor);

        //HandleFlags(request, flagValues, argsForFlags);

        await route.HandleAsync(request);
    }

    private Type GetImplementationType(ITemplatedRoute templatedRoute)
    {
        var implementedInterface = templatedRoute
            .GetType()
            .GetInterfaces()
            .Where(x => x.IsGenericType)
            .FirstOrDefault();

        if(implementedInterface == null)
        {
            return typeof(string[]);
        }

        var implementationTypeArguementType = implementedInterface
            .GetGenericArguments()[0];

        return implementationTypeArguementType;
    }

    //TODO: Move this out, use DI and a factory method / mapper
    private static Dictionary<string, ITemplatedRoute> ToDictionary(List<FullyQualifiedRoute> routes)
    {
        return routes.ToDictionary(
                x => x.RoutePath,
                x => x.Route
            );
    }

    private static string? GetDeepestKeyR(
            Dictionary<string, ITemplatedRoute> routeDictionary, 
            string currentKey)
    {
        if(String.IsNullOrEmpty(currentKey))
        {
            return null;
        }

        var hasRoute = routeDictionary.TryGetValue(currentKey, out var route);

        if(hasRoute)
        {
            return currentKey;
        }

        //Otherwise, lop off the end of the key
        var currentArgs = currentKey.Split(" ");

        var newArgs = currentArgs.SkipLast(1);

        var newKey = String.Join(" ", newArgs);

        return GetDeepestKeyR(routeDictionary, newKey);
    }
}
