using CliRouter.Core.Routes;

namespace CliRouter.Console.Routes.New;

public class TestRoutelet : IRoute
{
    public Task HandleAsync(string[] args)
    {
        System.Console.WriteLine("New Test called");

        return Task.CompletedTask;
    }
}
