
namespace Routing.Routes.First;

public class FirstLayerChild : IRoute
{
    public async Task HandleAsync(string[] args)
    {
        Console.WriteLine("First layer child has been invoked with args:");
        foreach(var arg in args)
        {
            Console.WriteLine(arg);
        }
    }
}
