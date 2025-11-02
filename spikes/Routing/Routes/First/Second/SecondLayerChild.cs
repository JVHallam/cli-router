
namespace Routing.Routes.First.Second;

public class SecondLayerChild : IRoute
{
    public async Task HandleAsync(string[] args)
    {
        Console.WriteLine("Second Layer Child has been called with args");
        foreach(var arg in args)
        {
            Console.WriteLine(arg);
        }
    }
}
