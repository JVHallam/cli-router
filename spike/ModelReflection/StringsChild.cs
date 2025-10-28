using Interfaces;
using Models;

//This is an example of doing things as I currently do!
public class StringsChild : IHandler
{
    public void Handle(string[] values)
    {
        Console.WriteLine("Stringy Child has been invoked");

        foreach(var value in values)
        {
            Console.WriteLine($"Arg : {value}");
        }
    }
}
