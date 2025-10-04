namespace CliRouter.Core.Routes;

public class RootRoute : RouterBase
{
    public override string Name => "Root"; 

    public RootRoute(IRootRoutelet[] routes) : base(routes)
    {
    }
}
