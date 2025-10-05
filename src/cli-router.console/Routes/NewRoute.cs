using CliRouter.Core.Routes;

namespace CliRouter.Console.Routes.New;

//Remove the IRoute here
public class NewRoute : RouteBase, IRootRoutelet
{
    public override string Name => "new";

    public NewRoute(IEnumerable<INewRoutelet> routelets) : base (routelets)
    {
    }
}
