using CliRouter.Core.Routes;

namespace CliRouter.Core.Services;

public class RouteKeyService : IRouteKeyService
{
    //TODO: Route dictionary should be injected into the class, rather than the method
    public string GetDeepestKey(
            Dictionary<string, ITemplatedRoute> routeDictionary,
            string[] allKeys)
    {
        var allKeysString = String.Join(" ", allKeys);

        return GetDeepestKey(routeDictionary, allKeysString);

    }

    private string GetDeepestKey(
            Dictionary<string, ITemplatedRoute> routeDictionary,
            string currentKey)
    {
        if (String.IsNullOrEmpty(currentKey))
        {
            throw new NotImplementedException($"No match was found for mapping the input key onto the routes");
        }

        var hasRoute = routeDictionary.TryGetValue(currentKey, out var route);

        if (hasRoute)
        {
            return currentKey;
        }

        //Otherwise, lop off the end of the key
        var currentArgs = currentKey.Split(" ");

        var newArgs = currentArgs.SkipLast(1);

        var newKey = String.Join(" ", newArgs);

        return GetDeepestKey(routeDictionary, newKey);
    }
}
