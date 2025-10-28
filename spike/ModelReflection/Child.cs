using Interfaces;
using Models;

//Child is the leaf node, It handles the calling off to business logic.
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
