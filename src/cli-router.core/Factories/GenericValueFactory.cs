using CliRouter.Core.Models;
using CliRouter.Core.Attributes;
using System.Reflection;

namespace CliRouter.Core.Factories;

public record FlagValue(
    string FlagName,
    string Value
);

public static class GenericValueFactory
{
    public static List<GenericValue> Create(Type type, string[] values)
    {
        var nonFlagValues = FilterOutFlags(values);
        var constructorGenericValues = GetConstructorGenericValues(type, nonFlagValues);

        var flagValues = FilterForFlags(values);
        var flagGenericValues = GetFlagGenericValues(type, flagValues);

        return constructorGenericValues
            .Concat(flagGenericValues)
            .ToList();
    }

    //Given "one", "--flag", "two", "three"
    //When called
    //Then filter for "one", "three"
    private static string[] FilterOutFlags(string[] values)
    {
        var wantedValues = new List<string>();

        //TODO: Gross, ew.
        bool wasLastValueAFlag = false;
        foreach(var value in values)
        {
            if(value.StartsWith("--"))
            {
                wasLastValueAFlag = true;
                continue;
            }

            if(wasLastValueAFlag)
            {
                wasLastValueAFlag = false;
                continue;
            }

            wasLastValueAFlag = false;
            wantedValues.Add(value);
        }

        return wantedValues.ToArray();
    }

    private static List<FlagValue> FilterForFlags(string[] values)
    {
        var wantedValues = new List<string>();

        //TODO: Gross, ew.
        bool wasLastValueAFlag = false;
        foreach(var value in values)
        {
            if(value.StartsWith("--"))
            {
                wasLastValueAFlag = true;
                var flagName = value.Remove(0, 2);
                wantedValues.Add(flagName);
                continue;
            }

            if(wasLastValueAFlag)
            {
                wasLastValueAFlag = false;
                wantedValues.Add(value);
                continue;
            }

            wasLastValueAFlag = false;
        }

        //Next, you want to zip them into FlagValue classes
        var wantedFlagValues = new List<FlagValue>();

        //Given an array "flag-name", "flag-value", "flag-name", "flag-value";
        //Skip along 2 at a time
        for(int i = 0; i < wantedValues.Count; i += 2)
        {
            var flagName = wantedValues[i];
            var flagValue = wantedValues[i + 1];

            wantedFlagValues.Add(new FlagValue(flagName, flagValue));
        }

        return wantedFlagValues;
    }

    private static List<GenericValue> GetConstructorGenericValues(Type type, string[] values)
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
            .Zip(values, (a, b) => new GenericValue(a,b,false))
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

        if(matchingFlagValue is null)
        {
            return null;
        }

        return new GenericValue(property.PropertyType, matchingFlagValue.Value, true);
    }
}
