using CliRouter.Core.Routes;
using CliRouter.Tests.Integration.TestClasses;

namespace CliRouter.Tests.Integration.Routes.Child;

/*
    I have this class, and HandleAsync takes a model, not a string[] array.

    First of all, I'm going to need to use reflection to know what the model is.
    Second of all, I'm going to need to know how many args that record takes
*/

public record RequestModel(
        string Arg1,
        string Arg2,
        string Arg3
);

public class ModelGrandChildRoutelet : IChildRoutelet, ITestableRoutelet
{
    private readonly List<Invocation> _invocations;

    public List<Invocation> Invocations
        => _invocations;

    public string Name => "model";

    public ModelGrandChildRoutelet()
    {
        _invocations = new List<Invocation>();
    }

    public Task HandleAsync(RequestModel request)
    {
        _invocations.Add(new Invocation(new string[]{}));

        Console.WriteLine("I was called lmfao, how? I have a model!?");

        return Task.CompletedTask;
    }
}
