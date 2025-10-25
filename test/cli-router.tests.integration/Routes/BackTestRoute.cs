using CliRouter.Core.Routes;
using CliRouter.Tests.Integration.Routes.BackTest;

namespace CliRouter.Tests.Integration.Routes;

public class BackTestRoute : RouteBase, IRootRoutelet
{
    public override string Name => "backtest";

    public BackTestRoute(IEnumerable<IBackTestRoutelet> routelets) : base (routelets)
    {
    }
}
