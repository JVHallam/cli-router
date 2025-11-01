using CliRouter.Core.Extensions;
using CliRouter.Core.Models;
using CliRouter.Core.Routes;
using CliRouter.Tests.Integration.Routes.Child;
using CliRouter.Tests.Integration.TestClasses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace CliRouter.Tests.Integration;

public class AllTests
{
    private readonly RootRoute _sut;

    public AllTests()
    {

        using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddCliRouting(Assembly.GetExecutingAssembly());
            })
            .Build();

        _sut = host
            .Services
            .GetRequiredService<RootRoute>();
    }

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
            "--flag-1",
            "flagonevalue",
            "1",
            "--flag-2",
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
        Assert.Equal(invocationArgs.Arg1, expectedArg1);
        Assert.Equal(invocationArgs.Arg2, expectedArg2);
        Assert.Equal(invocationArgs.Flag1, expectedFlag1);
        Assert.Equal(invocationArgs.Flag2, expectedFlag2);
    }

    [Fact]
    public async Task GivenAFlaggedRouteCalledWithoutFlags_WhenHandleAsyncCalled_ThenParsesArgsAsExpected()
    {
        await Task.CompletedTask;
    }
}
