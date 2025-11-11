namespace CliRouter.Core.Services;

public interface IArgsService
{
    string[] GetRouteArgsWithoutRoute(string fullRouteKey, string[] allArgs);
    string[] RemoveFlags(string[] allArgs);
    string[] GetFlags(string[] allArgs);
}
