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

        Console.WriteLine("Final route keys:");
        foreach(var key in routeDictionary.Keys)
        {
            Console.WriteLine($"{key}");
        }
    }
}
