using CliRouter.Core.Models;

namespace CliRouter.Core.Factories;

public static class GenericValueFactory
{
    public static List<GenericValue> Create(Type type, string[] values)
    {
        var constructor = type
            .GetConstructors()
            .FirstOrDefault();

        if(constructor == null)
        {
            throw new NotImplementedException($"Target type {type} requires a public constructor");
        }

        var parameters = constructor
            .GetParameters();

        var genericValues = parameters
            .Select(x => x.ParameterType)
            .Zip(values, (a, b) => new GenericValue(a,b))
            .ToList(); 

        return genericValues;
    }
}
