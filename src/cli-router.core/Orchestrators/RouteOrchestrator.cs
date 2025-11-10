using System.Collections.Generic;
using CliRouter.Core.Factories;
using CliRouter.Core.Routes;

namespace CliRouter.Core.Orchestrators;

public class RouteOrchestrator
{
    private readonly IDynamicFactory _dynamicFactory;
    private Dictionary<string, ITemplatedRoute> _routes;
    private IGenericValueFactory _genericValueFactory;
    private IObjectFactory _objectFactory;

    //TODO: Should a constructor be THIS complex?
    public RouteOrchestrator(
        IDynamicFactory dynamicFactory,
        IEnumerable<ITemplatedRoute> routes,
        IFullyQualifiedRouteFactory fullyQualifiedRouteFactory,
        IGenericValueFactory genericValueFactory,
        IObjectFactory objectFactory
    )
    {
        _dynamicFactory = dynamicFactory;
        _genericValueFactory = genericValueFactory;
        _objectFactory = objectFactory;

        //TODO: Also tolist when I can avoid it
        var fullyQualifiedRoutes = fullyQualifiedRouteFactory.Create(routes.ToList());
        _routes = ToDictionary(fullyQualifiedRoutes);
    }

    public async Task HandleAsync(string[] args)
    {
        var argsAsString = String.Join(" ", args);

        var deepestKey = GetDeepestKeyR(_routes, argsAsString);

        if (deepestKey == null)
        {
            Console.WriteLine("No key was matched!");
            return;
        }

        var route = _routes[deepestKey]!;

        var deepestKeyLength = deepestKey.Split(" ").Length;

        //Remove that from the args
        var rightArgs = args
            .Skip(deepestKeyLength)
            .ToArray();

        //Now we need to convert that into an object, if the route requires it
        var implementationTypeArgument = GetImplementationType(route);

        var genericValues = _genericValueFactory.Create(implementationTypeArgument, rightArgs);

        var constructorValues = genericValues
            .Where(x => !x.IsFlag)
            .ToList();

        var flagValues = genericValues
            .Where(x => x.IsFlag)
            .ToList();

        var argsForConstructor = _objectFactory.Create(constructorValues);

        var argsForFlags = _objectFactory.Create(flagValues);

        var request = _dynamicFactory.CreateInstance(implementationTypeArgument, argsForConstructor);

        HandleFlags(request, flagValues, argsForFlags);

        await route.HandleAsync(request);
    }

    //TODO: Might be better inside of the TypeFinder/TypeManagerService
    private Type GetImplementationType(ITemplatedRoute templatedRoute)
    {
        var implementedInterface = templatedRoute
            .GetType()
            .GetInterfaces()
            .Where(x => x.IsGenericType)
            .FirstOrDefault();

        if (implementedInterface == null)
        {
            return typeof(string[]);
        }

        var implementationTypeArguementType = implementedInterface
            .GetGenericArguments()[0];

        return implementationTypeArguementType;
    }

    //TODO: Move this out, use DI and a factory method / mapper
    private static Dictionary<string, ITemplatedRoute> ToDictionary(List<FullyQualifiedRoute> routes)
    {
        return routes.ToDictionary(
                x => x.RoutePath,
                x => x.Route
            );
    }

    private static string? GetDeepestKeyR(
            Dictionary<string, ITemplatedRoute> routeDictionary,
            string currentKey)
    {
        if (String.IsNullOrEmpty(currentKey))
        {
            return null;
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

        return GetDeepestKeyR(routeDictionary, newKey);
    }

    //TODO: This just feels so wrong. So wrong.
    public void HandleFlags(
            dynamic targetObject,
            List<GenericValue> flagValues,
            Object[] flagsAsObjects)
    {
        for (int i = 0; i < flagValues.Count; ++i)
        {
            var propertyValue = flagValues[i];
            var objectValue = flagsAsObjects[i];

            try
            {
                var property = propertyValue.PropertyInfo;
                property.SetValue(targetObject, objectValue);
            }
            catch (Exception ex)
            {
                var message = $"Value: '{propertyValue.Value}' cannot be converted to type {propertyValue.PropertyInfo.PropertyType}";
                throw new Exception(message, ex);
            }
        }
    }
}
