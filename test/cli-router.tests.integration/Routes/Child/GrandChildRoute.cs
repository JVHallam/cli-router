using CliRouter.Core.Routes;
using CliRouter.Tests.Integration.TestClasses;

namespace CliRouter.Tests.Integration.Routes.Child;

public class GrandChildRoute : IRoute
{
    private readonly InvocationTracker _invocationTracker;

    public GrandChildRoute(InvocationTracker invocationTracker)
    {
        _invocationTracker = invocationTracker;
    }

    public Task HandleAsync(string[] args)
    {
        _invocationTracker.Invocations.Add(new GenericInvocation<Object>(args, this));

        return Task.CompletedTask;
    }
}
