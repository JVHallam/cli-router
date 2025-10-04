using System.Collections.Generic;
using System.Linq;

namespace CliRouter.Core.Routes;

public abstract class RouterBase : IRoute
{
    public abstract string Name { get; }

    private readonly Dictionary<string, IRoute> _subRoutes;

    public RouterBase(IEnumerable<IRoute> routes)
    {
        _subRoutes = routes.ToDictionary(x => x.Name, x => x);
    }

    //The args coming in should just be not "rsis config new etc." but trimmed "new etc."
    public async Task HandleAsync(string[] args)
    {
        System.Console.WriteLine($"Route taken : {Name}");

        var requestedRouteName = args[0];

        var hasRoute = _subRoutes.TryGetValue(requestedRouteName, out IRoute requestedRoute);

        if(!hasRoute)
        {
            //TODO: Start using a logger over raw write lines 
            Console.WriteLine($"Route {Name} doesn't have sub route {requestedRouteName}");
            return;
        }

        var trimmedArgs = args.Skip(1).ToArray();

        await requestedRoute.HandleAsync(trimmedArgs);
    }
}
