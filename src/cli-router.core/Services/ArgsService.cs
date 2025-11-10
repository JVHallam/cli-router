namespace CliRouter.Core.Services;

public class ArgsService : IArgsService
{
    public string[] GetRouteArgsWithoutRoute(string fullRouteKey, string[] allArgs)
    {
        var deepestKeyLength = fullRouteKey.Split(" ").Length;

        return allArgs
            .Skip(deepestKeyLength)
            .ToArray();
    }
}
