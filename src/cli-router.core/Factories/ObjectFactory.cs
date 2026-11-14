using CliRouter.Core.Models;
using System.Linq;
using System.Globalization;

namespace CliRouter.Core.Factories;

public class ObjectFactory : IObjectFactory
{
    //TODO: Should really be a set of micro classes tbh
    private static Dictionary<Type, Func<string, Object>> _handlers
        => new Dictionary<Type, Func<string, Object>>()
        {
            { typeof(Int32), (input) => Int32.Parse(input) },
            { typeof(decimal), (input) => Decimal.Parse(input) },
            { typeof(string), (input) => input },
            { typeof(DateTime), (input) => DateTime.Parse(input) },
            { typeof(DateOnly), (input) => DateOnly.Parse(input) },

            //TODO: This is starting to get messy
            //Assuming all the args have been turned back 
            //into a single space deliminated string 
            { typeof(string[]), (input) => input.Split(" ")}
        };

    public Object[] Create(List<GenericValue> genericValues)
    {
        return genericValues
            .Select(Create)
            .ToArray();
    }

    public Object Create(GenericValue genericValue)
    {
        var (type, value, isFlag) = genericValue;

        var underlyingType = GetUnderlyingType(genericValue);

        var hasHandler = _handlers.TryGetValue(underlyingType, out var handler);

        if(underlyingType.IsEnum)
        {
            Type underlying = Enum.GetUnderlyingType(underlyingType);
            object number = Convert.ChangeType(value, underlying, CultureInfo.InvariantCulture);
            return Enum.ToObject(underlyingType, number);
        }

        if (!hasHandler || handler == null)
        {
            throw new NotImplementedException($"Type : {type} not supported. Only built-in types and their nullables + enums are supported");
        }

        try
        {
            return handler(value);
        }
        catch (FormatException ex)
        {
            throw new FormatException($"{value} - could not be parsed into type {type}", ex);
        }
    }

    private Type GetUnderlyingType(GenericValue genericValue)
    {
        var (type, _, __) = genericValue;

        var underlyingType = Nullable.GetUnderlyingType(type);

        if(underlyingType != null)
        {
            return underlyingType;
        }

        return genericValue.Type;
    }
}
