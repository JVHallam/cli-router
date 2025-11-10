using CliRouter.Core.Attributes;

namespace CliRouter.Tests.Integration.TestClasses;

//TODO:
//I don't like that this is Object. 
//Make Generic Invocation just called Invocation, with : Type, Object, Args
//Discard the generics
public class InvocationTracker
{
    public readonly List<GenericInvocation<Object>> Invocations;

    public InvocationTracker()
    {
        Invocations = new List<GenericInvocation<Object>>();
    }
}
