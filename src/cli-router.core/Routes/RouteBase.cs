using System.Collections.Generic;
using System.Linq;

namespace CliRouter.Core.Routes;

//TODO: Delete unused class
public abstract class RouteBase : IRoute
{
    public abstract string Name { get; }

    private readonly Dictionary<string, IRoute> _subRoutes;

    public RouteBase(IEnumerable<IRoute> routes)
    {
        //_subRoutes = routes.ToDictionary(x => x.Name, x => x);
    }

    public async Task HandleAsync(string[] args)
    {
        if(args.Length < 1)
        {
            Console.WriteLine($"Route {Name} requires args");
            return;
        }

        var requestedRouteName = args[0];

        var hasRoute = _subRoutes.TryGetValue(requestedRouteName, out IRoute? requestedRoute);

        if(!hasRoute || requestedRoute is null)
        {
            //TODO: Start using a logger over raw write lines 
            Console.WriteLine($"Route {Name} doesn't have sub route {requestedRouteName}");
            return;
        }

        var trimmedArgs = args.Skip(1).ToArray();

        await requestedRoute.HandleAsync(trimmedArgs);
    }

    public IRoute[] GetChildRoutes()
        => _subRoutes.Values.ToArray();
}
