using CliRouter.Core.Extensions;
using CliRouter.Core.Models;
using CliRouter.Core.Routes;
using CliRouter.Core.Orchestrators;
using CliRouter.Tests.Integration.Routes.Child;
using CliRouter.Tests.Integration.TestClasses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace CliRouter.Tests.Integration;

public class AllTests
{
    private readonly RouteOrchestrator _sut;
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
            .GetRequiredService<RouteOrchestrator>();
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

    [Fact]
    public async Task GivenASubRouteThatTakesAModel_WhenHandleAsyncCalled_ThenConvertsArgsIntoObject()
    {
        //Given
        var args = new string[]{ "child", "model-grand-child", "hello-world!", "100", "2025-05-05" };
        var expectedArg1 = "hello-world!";
        var expectedArg2 = 100;
        var expectedArg3 = DateOnly.Parse("2025-05-05");

        //When
        await _sut.HandleAsync(args);

        //Then
        Assert.Single(_invocationTracker.Invocations);
        
        var invocation = _invocationTracker.Invocations.First();
        var invokedTypeName = invocation.InvokedRoute.GetType().Name;
        Assert.Equal("ModelGrandChildRoute", invokedTypeName);

        var invocationArgs = (invocation.Args as RequestModel)!;
        Assert.Equal(invocationArgs.Arg1, expectedArg1);
        Assert.Equal(invocationArgs.Arg2, expectedArg2);
        Assert.Equal(invocationArgs.Arg3, expectedArg3);
    }

    [Fact]
    public async Task GivenACommandWithFlags_WhenHandleAsyncCalled_ThenParsesArgsAsExpected()
    {
        //Given
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

        //When
        await _sut.HandleAsync(args);

        //Then
        Assert.Single(_invocationTracker.Invocations);
        
        var invocation = _invocationTracker.Invocations.First();
        var invokedTypeName = invocation.InvokedRoute.GetType().Name;
        Assert.Equal("FlagsGrandChildRoute", invokedTypeName);

        var invocationArgs = (invocation.Args as FlagsModel)!;
        Assert.Equal(expectedArg1, invocationArgs.Arg1);
        Assert.Equal(expectedArg2, invocationArgs.Arg2);
        Assert.Equal(expectedFlag1, invocationArgs.Flag1);
        Assert.Equal(expectedFlag2, invocationArgs.Flag2);
    }
}
