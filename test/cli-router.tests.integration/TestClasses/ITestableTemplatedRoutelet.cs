namespace CliRouter.Tests.Integration.TestClasses;

public interface ITestableTemplatedRoutelet<T>
{
    List<GenericInvocation<T>> Invocations { get; }
}
