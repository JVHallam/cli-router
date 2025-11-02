using Routing.Routes;
using Routing.Routes.First;
using Routing.Routes.First.Second;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public static class Main
{
    public static async Task RunAsync()
    {
        //This routes to Routes/TopLayer.cs
        var args = new string[]{ "top-layer", "one", "two", "three" };

        //This routes to Routes/First/FirstLayerChild
        //var args = new string[]{ "first", "first-layer-child", "one", "two", "three" };

        var argsAsString = String.Join(" ", args);

        var routes = new List<ITemplatedRoute>()
        {
            new TopLayer(),
            new FirstLayerChild(),
            new SecondLayerChild()
        };

        var fullyQualifiedRoutes = FullyQualifiedRouteFactory.Create(routes);

        var routeDictionary = ToDictionary(fullyQualifiedRoutes);

        Console.WriteLine("Final route keys:");
        foreach(var key in routeDictionary.Keys)
        {
            Console.WriteLine($"{key}");
        }

        var deepestKey = GetDeepestKeyR(routeDictionary, argsAsString);

        Console.WriteLine($"Key that was matched: {deepestKey}");

        if(deepestKey == null)
        {
            Console.WriteLine("No key was matched!");
            return;
        }

        var route = routeDictionary[deepestKey]!;
        
        var deepestKeyLength = deepestKey.Split(" ").Length;

        //Remove that from the args
        var rightArgs = args
            .Skip(deepestKeyLength)
            .ToArray();

        await route.HandleAsync(rightArgs);
    }

    public static Dictionary<string, ITemplatedRoute> ToDictionary(List<FullyQualifiedRoute> routes)
    {
        Console.WriteLine("Routes:");
        foreach(var route in routes)
        {
            Console.WriteLine(route.RoutePath);
        }

        return routes
            .ToDictionary(
                x => x.RoutePath,
                x => x.Route
            );
    }

    public static string? GetDeepestKeyR(
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
