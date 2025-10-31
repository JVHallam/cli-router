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
        var grandChildRoute = (grandChildRoutes[0] as ITestableRoutelet)!;

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
        var args = new string[]{ "child", "model-grand-child", "hello world!", "100", "2025-05-05" };
        var expectedArg1 = "hello world!";
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
    public async Task Given_When_Then2()
    {
        //I can move onto removing Name from being an override and being a free property?!
        //I would like this, it's a major quality of life
        await Task.CompletedTask;
    }

    [Fact]
    public async Task Given_When_Then3()
    {
        //I CAN WRITE SOME INTEGRATION TESTS FOR HANDLING FLAGS!
        await Task.CompletedTask;
    }

    [Fact]
    public async Task Given_When_Then6()
    {
        //If routes have a help override
        //We could then output that
        //command help 
        /*
           Given a command "my-command"
           and a sub command "list" with the help field overriden
           and a sub command "delete" that's not overriden the help field
           When I invoke "my-command help"
           Then I expect to see:

           my-command:
                list: lists things
                delete
       */
        await Task.CompletedTask;
    }

    [Fact]
    public async Task Given_When_Then5()
    {
        //I would like to not have to create the Routers 
        //They should be dynamic created based on either namespace or folder path
        await Task.CompletedTask;
    }

}
