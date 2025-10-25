using CliRouter.Core.Routes;

namespace CliRouter.Tests.Integration.Routes.BackTest;

public class TestRoute : IBackTestRoutelet
{
    public string Name => "change this later";

    public async Task HandleAsync(string[] args)
    {
        Console.WriteLine("I was called lmfao");
    }
}
