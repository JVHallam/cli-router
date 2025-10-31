using CliRouter.Core.Routes;
using CliRouter.Tests.Integration.TestClasses;

namespace CliRouter.Tests.Integration.Routes.Child;

public class ModelGrandChildRoutelet : TemplatedRoutelet<RequestModel>, IChildRoutelet, ITestableTemplatedRoutelet<RequestModel>
{
    private readonly List<GenericInvocation<RequestModel>> _invocations;

    public List<GenericInvocation<RequestModel>> Invocations
        => _invocations;

    public override string Name => "model-grand-child";

    public ModelGrandChildRoutelet()
    {
        _invocations = new List<GenericInvocation<RequestModel>>();
    }

    public override Task HandleAsync(RequestModel request)
    {
        _invocations.Add(new GenericInvocation<RequestModel>(request));

        return Task.CompletedTask;
    }
}
