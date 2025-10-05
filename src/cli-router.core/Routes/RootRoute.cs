using System.Collections.Generic;

namespace CliRouter.Core.Routes;

public class RootRoute : RouteBase
{
    public override string Name => "Root Route"; 

    public RootRoute(IEnumerable<IRootRoutelet> routes) : base(routes)
    {
    }
}
