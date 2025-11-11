using CliRouter.Core.Models;
using CliRouter.Core.Attributes;
using System.Reflection;

namespace CliRouter.Core.Factories;

public class GenericValueFactory : IGenericValueFactory
{
    public List<GenericValue> Create(Type type, string[] values)
        => GetConstructorGenericValues(type, values);

    public List<GenericValue> Create(Type type, List<FlagValue> flagValues)
        => GetFlagGenericValues(type, flagValues);

    private static List<GenericValue> GetConstructorGenericValues(Type type, string[] values)
    {
        //TODO: Better handle all array types
        //Arrays and built in types are a little special
        //They can't be handled this way
        if (type == typeof(string[]))
        {
            //It'll be just the one string
            var singleString = String.Join(" ", values);
            return new List<GenericValue>()
            {
                new GenericValue(type, singleString, false)
            };
        }

        var constructor = type
            .GetConstructors()
            .FirstOrDefault();

        if (constructor == null)
        {
            throw new NotImplementedException($"Target type {type} requires a public constructor");
        }

        var parameters = constructor
            .GetParameters();

        var genericValues = parameters
            .Select(x => x.ParameterType)
            .Zip(values, (paramType, value) => new GenericValue(paramType, value, false))
            .ToList();

        return genericValues;
    }

    private static List<GenericValue> GetFlagGenericValues(Type type, List<FlagValue> flagValues)
    {
        var flagProperties = type
            .GetProperties()
            .Where(x => x.GetCustomAttribute<FlagAttribute>() != null);

        return flagProperties
            .Select(p => CreateGenericValue(p, flagValues))
            .Where(x => x != null)
            .Select(x => x!)
            .ToList();
    }

    //Remember to filter out the nulls later
    private static GenericValue? CreateGenericValue(PropertyInfo property, List<FlagValue> flagValues)
    {
        var flag = property.GetCustomAttribute<FlagAttribute>()!;

        var matchingFlagValue = flagValues.FirstOrDefault(x => x.FlagName == flag.Name);

        if (matchingFlagValue is null)
        {
            return null;
        }

        return new GenericValue(property.PropertyType, matchingFlagValue.Value, true)
        {
            PropertyInfo = property
        };
    }
}
