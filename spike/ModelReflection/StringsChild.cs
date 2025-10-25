using Interfaces;
using Models;

public class StringsChild : IGenericHandler<string[]>, IHandler
{
    public void Handle(Object values)
    {
        var actualValues = values as string[];

        Console.WriteLine("Stringy Child has been invoked");

        foreach(var value in actualValues)
        {
            Console.WriteLine($"Arg : {value}");
        }
    }
}
