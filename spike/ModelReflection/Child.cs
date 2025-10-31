using Interfaces;
using Models;

public class Child : GenericHandler<ChildRequest>
{
    public Child()
    {
    }

    public override void Handle(ChildRequest request)
    {
        Console.WriteLine("Child has been invoked");
        Console.WriteLine($"Arg1 : {request.Arg1}");
        Console.WriteLine($"Arg2 : {request.Arg2}");
    }
}
