using System.Text.RegularExpressions;

public static class FullyQualifiedRouteFactory
{
    public static List<FullyQualifiedRoute> Create(List<IRoute> routes)
    {
        return routes
            .Select(Create)
            .ToList();
    }

    public static FullyQualifiedRoute Create(IRoute route)
    {
        var routeName = GetRouteName(route);
        var routeParentNames = GetNamespaceRoute(route);

        var routePath = $"{routeParentNames} {routeName}".Trim();

        return new FullyQualifiedRoute()
        {
            RoutePath = routePath,
            Route = route
        };
    }

    private static string GetNamespaceRoute(IRoute route)
    {
        var type = route.GetType();
        var typeNamespace = type.Namespace;

        //Split on Routes
        var routeless = typeNamespace.Split(".Routes.");

        if(routeless.Length <= 1)
        {
            return String.Empty;
        }

        var routeNames = routeless[1].Split(".");

        var kebabRouteNames = routeNames
            .Select(ConvertToKebabCase)
            .ToArray();

        var asSingleString = String.Join(" ", routeNames);

        return asSingleString;
    }

    private static string GetRouteName(IRoute route)
    {
        var type = route.GetType();

        var typeName = type.Name;

        var kebabCaseName = ConvertToKebabCase(typeName);

        return kebabCaseName;
    }

    private static string ConvertToKebabCase(string pascalCaseString)
    {
        var pattern = "(?<!^)([A-Z])";

        string kebabCaseString = Regex
            .Replace(pascalCaseString, pattern, "-$1")
            .ToLower();

        return kebabCaseString;
    }
}
