using CliRouter.Core.Routes;

namespace CliRouter.Console.Routes.New;

public class NewRoute : RouteBase, IRootRoutelet
{
    public override string Name => "new";

    public NewRoute(IEnumerable<INewRoutelet> routelets) : base (routelets)
    {
    }
}
