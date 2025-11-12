namespace CliRouter.Core.Routes; 

public class BuiltInRouter : IBuiltInRouter
{
    private List<IBuiltInRoute> _routes;

    public BuiltInRouter(IEnumerable<IBuiltInRoute> builtInRoutes)
    {
        _routes = builtInRoutes.ToList();
    }

    public bool CanHandle(string[] allArgs)
    {
        var route = _routes.FirstOrDefault(x => x.CanHandle(allArgs));

        return route != null;
    }

    public async Task HandleAsync(
        Dictionary<string, ITemplatedRoute> routes,
        string[] allArgs)
    {
        var route = _routes.FirstOrDefault(x => x.CanHandle(allArgs));

        if(route == null)
        {
            throw new NotImplementedException("The given args cannot be handled by a built in route");
        }

        await route.HandleAsync(routes, allArgs);
    }
}
