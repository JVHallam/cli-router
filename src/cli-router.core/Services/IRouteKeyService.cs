using CliRouter.Core.Routes;

namespace CliRouter.Core.Services;

public interface IRouteKeyService
{
    public string GetDeepestKey(
            Dictionary<string, ITemplatedRoute> routeDictionary,
            string[] allKeys);
}
