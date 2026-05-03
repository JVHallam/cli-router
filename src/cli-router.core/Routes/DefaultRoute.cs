using System.Reflection;

namespace CliRouter.Core.Routes;

public class DefaultRoute : IBuiltInRoute
{
    public bool CanHandle(string[] allArgs)
    {
        return allArgs.Length == 0;
    }

    public Task HandleAsync(
            Dictionary<string, ITemplatedRoute> routes,
            string[] allArgs)
    {
        return Task.CompletedTask;
    }
}
