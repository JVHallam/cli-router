using CliRouter.Core.Routes;

namespace CliRouter.Core.Factories;

public interface IFullyQualifiedRouteFactory
{
    public List<FullyQualifiedRoute> Create(List<ITemplatedRoute> routes);
    public FullyQualifiedRoute Create(ITemplatedRoute route);
}
