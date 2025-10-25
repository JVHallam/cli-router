using System;
using System.Reflection;

public interface IHandler : IGenericHandler<string[]> { }

public interface IGenericHandler<T>
{
    public void Handle(T values);
}

public class Parent : IHandler
{
    private readonly Child _child;

    public Parent()
    {
        _child = new Child();
    }

    public void Handle(params string[] values)
    {
        //But how do we infer the type T of this
        var request = new ChildRequest("First arg");

        var implementationType = _child.GetType();

        var genericInterface = implementationType
            .GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGenericHandler<>));

        var recordType = genericInterface.GetGenericArguments()[0];

        var constructor = recordType.GetConstructors().First();

        foreach (var param in constructor.GetParameters())
        {
            Console.WriteLine($"{param.Name}: {param.ParameterType}");
        }

        Console.WriteLine($"Generic argument type: {recordType}");

        Activator.CreateInstance(recordType, values[0], values[1]);

        _child.Handle(request);
    }
}

public record ChildRequest(
    string Arg1,
    string Arg2
);

public class Child : IGenericHandler<ChildRequest>
{
    public Child()
    {
    }

    public void Handle(ChildRequest request)
    {
        Console.WriteLine("Child has been invoked");
        Console.WriteLine($"Arg1 : {request.Arg1}");
        Console.WriteLine($"Arg2 : {request.Arg2}");
    }
}

public class Main()
{
    public void Run()
    {
        var parent = new Parent();

        parent.Handle("Is", "This", "The", "Real", "Life");
    }
}
