using CliRouter.Core.Routes;
using CliRouter.Core.Attributes;
using CliRouter.Tests.Integration.TestClasses;

namespace CliRouter.Tests.Integration.Routes.Child;

[Help("This is the route for testing templated models, with flags")]
public class FlagsGrandChildRoute : ITemplatedRoute<FlagsModel>
{
    private readonly InvocationTracker _invocationTracker;

    public FlagsGrandChildRoute(InvocationTracker invocationTracker)
    {
        _invocationTracker = invocationTracker;
    }

    public Task HandleAsync(FlagsModel request)
    {
        _invocationTracker.Invocations.Add(new GenericInvocation<Object>(request, this));
        return Task.CompletedTask;
    }
}
