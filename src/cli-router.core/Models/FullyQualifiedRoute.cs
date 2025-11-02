using  CliRouter.Core.Routes;

namespace CliRouter.Core.Models;

public class FullyQualifiedRoute
{
    public required string RoutePath { get; set; }
    public required ITemplatedRoute Route { get; set; }
}
