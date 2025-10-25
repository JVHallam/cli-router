namespace CliRouter.Tests.Integration.TestClasses;

public record Invocation(
    //The args passed to it when HandleAsync was invoked
    string[] Args
);
