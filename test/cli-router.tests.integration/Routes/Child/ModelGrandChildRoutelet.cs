using CliRouter.Core.Routes;
using CliRouter.Tests.Integration.TestClasses;

namespace CliRouter.Tests.Integration.Routes.Child;

public class ModelGrandChildRoute : ITemplatedRoute<RequestModel>
{
    private readonly InvocationTracker _invocationTracker;

    public ModelGrandChildRoute(
        InvocationTracker invocationTracker
    )
    {
        _invocationTracker = invocationTracker;
    }

    public Task HandleAsync(RequestModel request)
    {
        _invocationTracker.Invocations.Add(new GenericInvocation<Object>(request, this));

        return Task.CompletedTask;
    }
}
