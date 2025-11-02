
namespace Routing.Routes;

public class TopLayer : IRoute
{
    public async Task HandleAsync(string[] args)
    {
        Console.WriteLine("Top layer has been called with args");
        foreach(var arg in args)
        {
            Console.WriteLine(arg);
        }
    }
}
