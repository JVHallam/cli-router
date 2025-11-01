using CliRouter.Core.Routes;
using CliRouter.Tests.Integration.TestClasses;

namespace CliRouter.Tests.Integration.Routes.Child;

public class FlagsGrandChildRoutelet : TemplatedRoutelet<FlagsModel>, IChildRoutelet, ITestableTemplatedRoutelet<FlagsModel>
{
    private readonly List<GenericInvocation<FlagsModel>> _invocations;

    public List<GenericInvocation<FlagsModel>> Invocations
        => _invocations;

    public override string Name => "flags-grand-child";

    public FlagsGrandChildRoutelet()
    {
        _invocations = new List<GenericInvocation<FlagsModel>>();
    }

    public override Task HandleAsync(FlagsModel request)
    {
        _invocations.Add(new GenericInvocation<FlagsModel>(request));

        return Task.CompletedTask;
    }
}
