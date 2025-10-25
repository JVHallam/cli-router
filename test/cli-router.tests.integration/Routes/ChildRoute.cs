using CliRouter.Core.Routes;
using CliRouter.Tests.Integration.Routes.Child;

namespace CliRouter.Tests.Integration.Routes;

public class ChildRoute : RouteBase, IRootRoutelet
{
    public override string Name => "child";

    public ChildRoute(IEnumerable<IChildRoutelet> routelets) : base (routelets)
    {
    }
}
