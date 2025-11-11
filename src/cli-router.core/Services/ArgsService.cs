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

    private record PairedArgs(
        string LeftArg,
        string RightArg
    );

    public string[] GetFlags(string[] allArgs)
    {
        var filteredArgs = new List<string>();

        var wasPreviousArgFlag = false;
        foreach(var arg in allArgs)
        {
            if(wasPreviousArgFlag)
            {
                wasPreviousArgFlag = false;
                filteredArgs.Add(arg);
                continue;
            }

            var isCurrentArgFlag = arg.Contains("--");

            if(isCurrentArgFlag)
            {
                wasPreviousArgFlag = true;
                filteredArgs.Add(arg);
                continue;
            }
        }

        return filteredArgs.ToArray();
    }

    public string[] RemoveFlags(string[] allArgs)
    {
        var filteredArgs = new List<string>();

        var wasPreviousArgFlag = false;
        foreach(var arg in allArgs)
        {
            if(wasPreviousArgFlag)
            {
                wasPreviousArgFlag = false;
                continue;
            }

            var isCurrentArgFlag = arg.Contains("--");

            if(isCurrentArgFlag)
            {
                wasPreviousArgFlag = true;
                continue;
            }

            filteredArgs.Add(arg);
        }

        return filteredArgs.ToArray();
    }
}
