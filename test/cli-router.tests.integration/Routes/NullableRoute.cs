using CliRouter.Core.Routes;
using CliRouter.Core.Attributes;
using CliRouter.Tests.Integration.TestClasses;

namespace CliRouter.Tests.Integration.Routes;

[Help("This is the route for testing templated models, with nullable flags")]
public class NullableRoute : ITemplatedRoute<NullableFieldsRequest>
{
    private readonly InvocationTracker _invocationTracker;

    public NullableRoute(InvocationTracker invocationTracker)
    {
        _invocationTracker = invocationTracker;
    }

    public Task HandleAsync(NullableFieldsRequest request)
    {
        _invocationTracker.Invocations.Add(new GenericInvocation<Object>(request, this));
        return Task.CompletedTask;
    }
}
