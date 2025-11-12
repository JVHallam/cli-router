using CliRouter.Core.Services;
using CliRouter.Core.Attributes;
using System.Reflection;

namespace CliRouter.Core.Routes;

public class HelpRoute : IBuiltInRoute
{
    private readonly IGenericTypeService _genericTypeService;

    public HelpRoute(
        IGenericTypeService genericTypeService
    )
    {
        _genericTypeService = genericTypeService;
    }

    public bool CanHandle(string[] allArgs)
    {
        if(allArgs.FirstOrDefault(x => x == "--help") != null)
        {
            return true;
        }

        return false;
    }

    public Task HandleAsync(
            Dictionary<string, ITemplatedRoute> routes,
            string[] allArgs)
    {
        //Output all the routes:
        Console.WriteLine("All Routes:");
        foreach(var pair in routes)
        {
            Console.WriteLine(" ");
            Console.WriteLine($"Route : {pair.Key}");
            PrintInfo(pair.Value);
        }

        return Task.CompletedTask;
    }

    private void PrintInfo(ITemplatedRoute route)
    {
        var modelType = _genericTypeService.GetImplementationTypeArgument(route);

        PrintHelpAttribute(route);
        PrintConstructorArguments(modelType);
        PrintFlagArguments(modelType);
    }

    private void PrintHelpAttribute(ITemplatedRoute route)
    {
        var type = route.GetType();

        var helpAttribute = type.GetCustomAttribute<HelpAttribute>();

        if(helpAttribute == null)
        {
            return;
        }

        Console.WriteLine(helpAttribute.HelpText);
    }

    private void PrintConstructorArguments(Type modelType)
    {
        //We want to use reflection to output the values of the constructor
        var constructor = modelType
            .GetConstructors()
            .FirstOrDefault();

        if (constructor == null)
        {
            Console.WriteLine("Route has no args");
            return;
        }

        var parameters = constructor
            .GetParameters();

        Console.WriteLine("Route requires: ");
        foreach(var parameter in parameters)
        {
            var name = parameter.Name;
            var parameterType = parameter.ParameterType;
            Console.WriteLine($"{name} : {parameterType}");
        }
    }

    private void PrintFlagArguments(Type modelType)
    {
        Console.WriteLine("Flags:");

        var flagProperties = modelType
            .GetProperties()
            .Where(x => x.GetCustomAttribute<FlagAttribute>() != null);

        if(flagProperties.Count() == 0)
        {
            Console.WriteLine("None.");
            return;
        }

        //I want the flag name, not the property name
        foreach(var flag in flagProperties)
        {
            //var name = flag.Name;
            var flagAttribute = flag.GetCustomAttribute<FlagAttribute>();
            if(flagAttribute == null)
            {
                continue;
            }
            var name = flagAttribute.Name;
            var propertyType = flag.PropertyType;
            Console.WriteLine($"--{name} : {propertyType}");
        }
        /*
        return flagProperties
            .Select(p => CreateGenericValue(p, flagValues))
            .Where(x => x != null)
            .Select(x => x!)
            .ToList();
        */
    }
}
