using Models;
using System.Linq;

public static class ObjectFactory
{
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

        switch(typeCode)
        {
            case TypeCode.Int32:
                return Int32.Parse(value);
            case TypeCode.Int64:
                return Int64.Parse(value);
            case TypeCode.Decimal:
                return Decimal.Parse(value);
            case TypeCode.String:
                return value;
            case TypeCode.DateTime:
                return DateTime.Parse(value);
        }

        throw new NotImplementedException($"Type : {type} not supported. Only built-in types supported");
    }
}
