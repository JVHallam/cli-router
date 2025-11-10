using CliRouter.Core.Models;
using System.Linq;

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

        var typeCode = Type.GetTypeCode(type);

        var hasHandler = _handlers.TryGetValue(type, out var handler);

        if (!hasHandler)
        {
            throw new NotImplementedException($"Type : {type} not supported. Only built-in types supported");
        }

        try
        {
            return handler!(value);
        }
        catch (FormatException ex)
        {
            throw new FormatException($"{value} - could not be parsed into type {type}", ex);
        }
    }
}
