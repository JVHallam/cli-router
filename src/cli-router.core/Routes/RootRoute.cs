using System.Collections.Generic;

namespace CliRouter.Core.Routes;

public class RootRoute : RouterBase
{
    public override string Name => "Root"; 

    public RootRoute(IEnumerable<IRootRoutelet> routes) : base(routes)
    {
    }
}
