namespace CliRouter.Tests.Integration.TestClasses;

public record Invocation(string[] Args) 
    : GenericInvocation<string[]> (Args);
