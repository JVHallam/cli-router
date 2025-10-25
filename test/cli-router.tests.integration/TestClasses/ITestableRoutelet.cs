namespace CliRouter.Tests.Integration.TestClasses;

public interface ITestableRoutelet
{
    List<Invocation> Invocations { get; }
}
