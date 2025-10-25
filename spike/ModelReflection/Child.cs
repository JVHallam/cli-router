using Interfaces;
using Models;

public class Child : IGenericHandler<ChildRequest>
{
    public Child()
    {
    }

    public void Handle(Object request)
    {
        var actualRequest = request as ChildRequest;

        Console.WriteLine("Child has been invoked");
        Console.WriteLine($"Arg1 : {actualRequest.Arg1}");
        Console.WriteLine($"Arg2 : {actualRequest.Arg2}");
    }
}
