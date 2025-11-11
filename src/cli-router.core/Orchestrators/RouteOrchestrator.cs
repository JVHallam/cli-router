using System.Collections.Generic;
using CliRouter.Core.Factories;
using CliRouter.Core.Routes;
using CliRouter.Core.Services;

namespace CliRouter.Core.Orchestrators;

public class RouteOrchestrator
{
    private readonly IDynamicFactory _dynamicFactory;
    private Dictionary<string, ITemplatedRoute> _routes;
    private IGenericValueFactory _genericValueFactory;
    private IObjectFactory _objectFactory;
    private IGenericTypeService _genericTypeService;
    private IObjectMapper _objectMapper;
    private IRouteKeyService _routeKeyService;
    private IArgsService _argsService;

    //TODO: Should a constructor be THIS complex?
    public RouteOrchestrator(
        IDynamicFactory dynamicFactory,
        IEnumerable<ITemplatedRoute> routes,
        IFullyQualifiedRouteFactory fullyQualifiedRouteFactory,
        IGenericValueFactory genericValueFactory,
        IObjectFactory objectFactory,
        IGenericTypeService genericTypeService,
        IObjectMapper objectMapper,
        IRouteKeyService routeKeyService,
        IArgsService argsService
    )
    {
        _dynamicFactory = dynamicFactory;
        _genericValueFactory = genericValueFactory;
        _objectFactory = objectFactory;
        _genericTypeService = genericTypeService;
        _objectMapper = objectMapper;
        _routeKeyService = routeKeyService;
        _argsService = argsService;

        //TODO: Also tolist when I can avoid it
        //This feels like something that should be done elsewhere
        var fullyQualifiedRoutes = fullyQualifiedRouteFactory.Create(routes.ToList());
        _routes = _objectMapper.ToDictionary(fullyQualifiedRoutes);
    }

    public async Task HandleAsync(string[] args)
    {
        var argsWithoutFlags = _argsService.RemoveFlags(args);

        var flagsWithoutArgs = _argsService.GetFlags(args);

        var deepestKey = _routeKeyService.GetDeepestKey(_routes, argsWithoutFlags);

        var route = _routes[deepestKey]!;

        var rightArgs = _argsService.GetRouteArgsWithoutRoute(deepestKey, argsWithoutFlags);

        var implementationTypeArgument = _genericTypeService.GetImplementationTypeArgument(route);

        //This is now missing the flags
        //Add the flags back in
        var rightArgsList = rightArgs
            .ToList();

        rightArgsList.AddRange(flagsWithoutArgs);

        var rightArgsWithFlags = rightArgsList.ToArray();

        var genericValues = _genericValueFactory.Create(implementationTypeArgument, rightArgsWithFlags);

        var constructorValues = genericValues
            .Where(x => !x.IsFlag)
            .ToList();

        var flagValues = genericValues
            .Where(x => x.IsFlag)
            .ToList();

        var argsForConstructor = _objectFactory.Create(constructorValues);

        var argsForFlags = _objectFactory.Create(flagValues);

        var request = _dynamicFactory.CreateInstance(implementationTypeArgument, argsForConstructor);

        _objectMapper.MapFlagsOnto(request, flagValues, argsForFlags);

        await route.HandleAsync(request);
    }
}
