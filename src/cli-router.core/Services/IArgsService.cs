namespace CliRouter.Core.Services;

public interface IArgsService
{
    string[] GetRouteArgsWithoutRoute(string fullRouteKey, string[] allArgs);
}
