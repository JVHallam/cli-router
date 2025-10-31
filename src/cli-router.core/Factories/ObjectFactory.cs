using CliRouter.Core.Models;
using System.Linq;

namespace CliRouter.Core.Factories;

public static class ObjectFactory
{
    public static Dictionary<Type, Func<string, Object>> _handlers 
        => new Dictionary<Type, Func<string, Object>>()
        {
            { typeof(Int32), (input) => Int32.Parse(input) },
            { typeof(decimal), (input) => Decimal.Parse(input) },
            { typeof(string), (input) => input },
            { typeof(DateTime), (input) => DateTime.Parse(input) },
            { typeof(DateOnly), (input) => DateOnly.Parse(input) }
        };


    public static Object[] Create(List<GenericValue> genericValues)
    {
        return genericValues
            .Select(ObjectFactory.Create)
            .ToArray();
    }

    public static Object Create(GenericValue genericValue)
    {
        var ( type, value ) = genericValue;

        var typeCode = Type.GetTypeCode(type);

        var hasHandler = _handlers.TryGetValue(type, out var handler);

        if(!hasHandler)
        {
            throw new NotImplementedException($"Type : {type} not supported. Only built-in types supported");
        }

        return handler(value);
    }
}
