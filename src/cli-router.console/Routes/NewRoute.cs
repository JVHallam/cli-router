using CliRouter.Core.Routes;

namespace CliRouter.Console.Routes;

//Remove the IRoute here
public class NewRoute : IRootRoutelet
{
    public string Name => "new";

    public NewRoute()
    {
    }

    public async Task HandleAsync(string[] args)
    {
        System.Console.WriteLine("New called with following args!");
        foreach(var arg in args)
        {
            System.Console.WriteLine(arg);
        }
    }
}
