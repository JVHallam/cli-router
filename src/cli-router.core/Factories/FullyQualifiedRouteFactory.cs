using System.Text.RegularExpressions;
using CliRouter.Core.Routes;

namespace CliRouter.Core.Factories;

public class FullyQualifiedRouteFactory : IFullyQualifiedRouteFactory
{
    public List<FullyQualifiedRoute> Create(List<ITemplatedRoute> routes)
    {
        return routes
            .Select(Create)
            .ToList();
    }

    public FullyQualifiedRoute Create(ITemplatedRoute route)
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

    //More a string Factory method
    private static string GetNamespaceRoute(ITemplatedRoute route)
    {
        var type = route.GetType();

        var typeNamespaceInstance = type.Namespace;

        if(typeNamespaceInstance == null)
        {
            throw new NotImplementedException($"Route {route} must exist within a namespace");
        }

        var typeNamespace = typeNamespaceInstance.ToLower();

        //Split on Routes
        var routeless = typeNamespace.Split(".routes.");

        if (routeless.Length <= 1)
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

    //More a string factory method
    private static string GetRouteName(ITemplatedRoute route)
    {
        var type = route.GetType();

        var typeName = type.Name;

        var typeNameWithoutRoute = typeName.Replace("Route", "");

        var kebabCaseName = ConvertToKebabCase(typeNameWithoutRoute);

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
