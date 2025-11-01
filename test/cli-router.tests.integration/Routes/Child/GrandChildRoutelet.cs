using CliRouter.Core.Routes;
using CliRouter.Tests.Integration.TestClasses;

namespace CliRouter.Tests.Integration.Routes.Child;

public class GrandChildRoutelet : IChildRoutelet, ITestableRoutelet
{
    private readonly List<Invocation> _invocations;

    public List<Invocation> Invocations
        => _invocations;

    public string Name => "grand-child";

    public GrandChildRoutelet()
    {
        _invocations = new List<Invocation>();
    }

    public Task HandleAsync(string[] args)
    {
        _invocations.Add(new Invocation(args));

        Console.WriteLine("I was called lmfao");

        return Task.CompletedTask;
    }
}
