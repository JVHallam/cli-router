using CliRouter.Core.Extensions;
using CliRouter.Core.Models;
using CliRouter.Core.Routes;
using CliRouter.Tests.Integration.Routes.Child;
using CliRouter.Tests.Integration.TestClasses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace CliRouter.Tests.Integration;

public class InvocationTracker
{
    public readonly List<GenericInvocation<Object>> Invocations;

    public InvocationTracker()
    {
        Invocations = new List<GenericInvocation<Object>>();
    }
}

public class AllTests
{
    private readonly Router _sut;
    private readonly InvocationTracker _invocationTracker;

    public AllTests()
    {
        _invocationTracker = new InvocationTracker();

        using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton(_invocationTracker);
                services.AddCliRouting(Assembly.GetExecutingAssembly());
            })
            .Build();

        _sut = host
            .Services
            .GetRequiredService<Router>();
    }

    //DI works now, so we can just inject in the invocation class
    [Fact]
    public async Task GivenBasicArgs_WhenHandleAsyncCalled_ThenInvokesTargetRoute()
    {
        //Given
        var args = new string[]{ "child", "grand-child", "solo-arg" };

        //When
        await _sut.HandleAsync(args);

        //Then
        Assert.Single(_invocationTracker.Invocations);
        
        var invocation = _invocationTracker.Invocations.First();
        var invokedTypeName = invocation.InvokedRoute.GetType().Name;
        Assert.Equal("GrandChildRoute", invokedTypeName);
    }

    /*
    private ITestableTemplatedRoutelet<T> GetTemplatedRoutelet<T>(string name)
    {
        var childRoutes = _sut.GetChildRoutes();
        var childRoute = childRoutes[0] as RouteBase;

        var grandChildRoutes = childRoute!.GetChildRoutes();
        var grandChildRoute = (grandChildRoutes.First(x => x.Name == name) as ITestableTemplatedRoutelet<T>)!;

        return grandChildRoute;
    }

    private ITestableRoutelet GetRoutelet(string name)
    {
        var childRoutes = _sut.GetChildRoutes();
        var childRoute = childRoutes[0] as RouteBase;

        var grandChildRoutes = childRoute!.GetChildRoutes();
        var grandChildRoute = (grandChildRoutes.First(x => x.Name == name) as ITestableRoutelet)!;

        return grandChildRoute;
    }

    [Fact]
    public void GivenAddCliRoutingSetup_WhenServiceResolved_ThenSubroutesDetectedAndRegisteredSuccessfully()
    {
        //Given DI has setup and reflection has picked up sub classes
        //Root Route
        //-> ChildRoute
        //--> GrandChildRoutelet

        //When
        var childRoutes = _sut.GetChildRoutes();
        var childRoute = childRoutes[0] as RouteBase;

        var grandChildRoute = GetRoutelet("grand-child") as IChildRoutelet;

        //Then Expect
        Assert.NotNull(childRoute);
        Assert.NotNull(grandChildRoute);

        Assert.Equal("child", childRoute.Name);
        Assert.Equal("grand-child", grandChildRoute.Name);
    }

    [Fact]
    public async Task GivenASubRoute_WhenHandleAsyncCalled_ThenGrandChildRouteIsInvoked()
    {
        //Given
        var grandChildExpectedArgs = new string[]{ "arg1", "arg2" };
        var args = new string[]{ "child", "grand-child", "arg1", "arg2" };

        var grandChildRoute = GetRoutelet("grand-child");

        //When
        await _sut.HandleAsync(args);

        //Then
        var invocation = grandChildRoute.Invocations.FirstOrDefault();
        Assert.Single(grandChildRoute.Invocations);
        Assert.NotNull(invocation);

        var invocationArgs = invocation.Args;
        Assert.Equal(2, invocationArgs.Length);
        Assert.Equal(invocationArgs[0], grandChildExpectedArgs[0]);
        Assert.Equal(invocationArgs[1], grandChildExpectedArgs[1]);
    }

    [Fact]
    public async Task GivenASubRouteThatTakesAModel_WhenHandleAsyncCalled_ThenConvertsArgsIntoObject()
    {
        var args = new string[]{ "child", "model-grand-child", "hello-world!", "100", "2025-05-05" };
        var expectedArg1 = "hello-world!";
        var expectedArg2 = 100;
        var expectedArg3 = DateOnly.Parse("2025-05-05");

        //Given
        var childRoutes = _sut.GetChildRoutes();
        var childRoute = childRoutes[0] as RouteBase;

        var grandChildRoutes = childRoute!.GetChildRoutes();
        var grandChildRoute = (grandChildRoutes.First(x => x.Name == "model-grand-child") as ITestableTemplatedRoutelet<RequestModel>)!;

        //When
        await _sut.HandleAsync(args);

        //Then
        var invocation = grandChildRoute.Invocations.FirstOrDefault();
        Assert.Single(grandChildRoute.Invocations);
        Assert.NotNull(invocation);

        var invocationArgs = (invocation.Args as RequestModel)!;
        Assert.Equal(invocationArgs.Arg1, expectedArg1);
        Assert.Equal(invocationArgs.Arg2, expectedArg2);
        Assert.Equal(invocationArgs.Arg3, expectedArg3);
    }


    [Fact]
    public async Task GivenACommandWithFlags_WhenHandleAsyncCalled_ThenParsesArgsAsExpected()
    {
        var args = new string[]{ 
            "child", 
            "flags-grand-child",
            "arg1",
            "--flag-one",
            "flagonevalue",
            "1",
            "--flag-two",
            "5"
        };
        var expectedArg1 = "arg1";
        var expectedArg2 = 1;
        var expectedFlag1 = "flagonevalue";
        var expectedFlag2 = 5;

        //Given
        var grandChildRoute = GetTemplatedRoutelet<FlagsModel>("flags-grand-child");

        //When
        await _sut.HandleAsync(args);

        //Then
        var invocation = grandChildRoute.Invocations.FirstOrDefault();
        Assert.Single(grandChildRoute.Invocations);
        Assert.NotNull(invocation);

        var invocationArgs = (invocation.Args as FlagsModel)!;
        Assert.Equal(expectedArg1, invocationArgs.Arg1);
        Assert.Equal(expectedArg2, invocationArgs.Arg2);
        Assert.Equal(expectedFlag1, invocationArgs.Flag1);
        Assert.Equal(expectedFlag2, invocationArgs.Flag2);
    }
    */
}
