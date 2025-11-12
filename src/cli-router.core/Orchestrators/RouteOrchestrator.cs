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
    private IFlagValueFactory _flagValueFactory;
    private IBuiltInRouter _builtInRouter;

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
        IArgsService argsService,
        IFlagValueFactory flagValueFactory,
        IBuiltInRouter builtInRouter
    )
    {
        _dynamicFactory = dynamicFactory;
        _genericValueFactory = genericValueFactory;
        _objectFactory = objectFactory;
        _genericTypeService = genericTypeService;
        _objectMapper = objectMapper;
        _routeKeyService = routeKeyService;
        _argsService = argsService;
        _flagValueFactory = flagValueFactory;
        _builtInRouter = builtInRouter;

        //TODO: Also tolist when I can avoid it
        //This feels like something that should be done elsewhere
        var fullyQualifiedRoutes = fullyQualifiedRouteFactory.Create(routes.ToList());
        _routes = _objectMapper.ToDictionary(fullyQualifiedRoutes);
    }

    public async Task HandleAsync(string[] args)
    {
        if(_builtInRouter.CanHandle(args))
        {
            await _builtInRouter.HandleAsync(_routes, args);
            return;
        }

        //Handle the routing
        var argsWithoutFlags = _argsService.RemoveFlags(args);
        var deepestKey = _routeKeyService.GetDeepestKey(_routes, argsWithoutFlags);
        var route = _routes[deepestKey]!;

        //Handle the constructor args
        var rightArgs = _argsService.GetRouteArgsWithoutRoute(deepestKey, argsWithoutFlags);
        var implementationTypeArgument = _genericTypeService.GetImplementationTypeArgument(route);

        //Handle the constructor args
        var constructorValues = _genericValueFactory.Create(implementationTypeArgument, rightArgs);
        var argsForConstructor = _objectFactory.Create(constructorValues);
        var request = _dynamicFactory.CreateInstance(implementationTypeArgument, argsForConstructor);

        //Handle the Flags
        var flagsWithoutArgs = _argsService.GetFlags(args);
        var flagValuesList = _flagValueFactory.Create(flagsWithoutArgs);
        var flagValues = _genericValueFactory.Create(implementationTypeArgument, flagValuesList);
        var argsForFlags = _objectFactory.Create(flagValues);
        _objectMapper.MapFlagsOnto(request, flagValues, argsForFlags);

        await route.HandleAsync(request);
    }
}
