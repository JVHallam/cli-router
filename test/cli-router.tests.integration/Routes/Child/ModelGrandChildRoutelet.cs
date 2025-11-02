using CliRouter.Core.Routes;
using CliRouter.Tests.Integration.TestClasses;

namespace CliRouter.Tests.Integration.Routes.Child;

public class ModelGrandChildRoute : ITemplatedRoute<RequestModel>, ITestableTemplatedRoutelet<RequestModel>
{
    private readonly List<GenericInvocation<RequestModel>> _invocations;

    public List<GenericInvocation<RequestModel>> Invocations
        => _invocations;

    public ModelGrandChildRoute()
    {
        _invocations = new List<GenericInvocation<RequestModel>>();
    }

    public Task HandleAsync(RequestModel request)
    {
        _invocations.Add(new GenericInvocation<RequestModel>(request, this));

        return Task.CompletedTask;
    }
}
