using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CliRouter.Core.Extensions;
using CliRouter.Core.Routes;
using CliRouter.Core.Models;
using System.Reflection;
using CliRouter.Tests.Integration.TestClasses;

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

        var grandChildRoutes = childRoute!.GetChildRoutes();
        var grandChildRoute = grandChildRoutes[0];

        //Then Expect
        Assert.NotNull(childRoute);
        Assert.NotNull(grandChildRoute);

        Assert.Equal("child", childRoute.Name);
        Assert.Equal("grandchild", grandChildRoute.Name);
    }

    [Fact]
    public async Task GivenASubRoute_WhenHandleAsyncCalled_ThenGrandChildRouteIsInvoked()
    {
        var grandChildExpectedArgs = new string[]{ "arg1", "arg2" };
        var args = new string[]{ "child", "grandchild", "arg1", "arg2" };

        //Given
        var childRoutes = _sut.GetChildRoutes();
        var childRoute = childRoutes[0] as RouteBase;

        var grandChildRoutes = childRoute!.GetChildRoutes();
        var grandChildRoute = grandChildRoutes[0] as ITestableRoutelet;

        //When
        await _sut.HandleAsync(args);

        //Then
        var invocation = grandChildRoute.Invocations.FirstOrDefault();
        Assert.Equal(1, grandChildRoute.Invocations.Count);
        Assert.NotNull(invocation);

        var invocationArgs = invocation.Args;
        Assert.Equal(2, invocationArgs.Length);
        Assert.Equal(invocationArgs[0], grandChildExpectedArgs[0]);
        Assert.Equal(invocationArgs[1], grandChildExpectedArgs[1]);
    }

    [Fact]
    public async Task GivenASubRouteThatTakesAModel_WhenHandleAsyncCalled_ThenConvertsArgsIntoObject()
    {

    }
}
