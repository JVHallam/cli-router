using Models;
using Interfaces;

public class Parent
{
    private readonly IHandler _child;

    public Parent()
    {
        _child = new Child();
    }

    public void Handle(params string[] values)
    {
       /*
            TODO:
            - The children should implement an abstract base class
            - The abstract base class has Handle(string[] args)
            - The abstract base class then does all the casting and stuff
            - The abstract base class then invokes Handle(T request)
            - That way the parent can do all the routing
            - The child can do all the type manipulation
            - Then the user gets to do Handle(T request) and just handle the actual logic, not the type safe logic
       */
        var implementationType = _child.GetType();

        var implementationTypeArgument = GetChildsImplementationTypeArgument(implementationType);

        var genericValues = CreateGenericValues(implementationTypeArgument, values);

        var argsForConstructor = ConvertToParams(genericValues);

        dynamic request = Activator.CreateInstance(implementationTypeArgument, argsForConstructor);

        _child.Handle(request);
    }

    //Given a Generic Type, and some strings, create the GenericValues
    private static List<GenericValue> CreateGenericValues(Type type, string[] values)
    {
        var constructor = type.GetConstructors().First();

        var parameters = constructor.GetParameters();

        var genericValues = parameters
            .Select(x => x.ParameterType)
            .Zip(values, (a, b) => new GenericValue(a,b))
            .ToList(); 

        return genericValues;
    }

    private static Type GetChildsImplementationTypeArgument(Type type)
    {
        var genericInterface = type
            .GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGenericHandler<>));

        var recordType = genericInterface.GetGenericArguments()[0];

        return recordType;
    }

    private static Object[] ConvertToParams(List<GenericValue> values)
    {
        return values
            .Select(ConvertToBuiltInType)
            .ToArray();
    }

    private static Object ConvertToBuiltInType(GenericValue genericValue)
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
