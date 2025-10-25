using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CliRouter.Core.Extensions;
using CliRouter.Core.Routes;
using CliRouter.Core.Models;
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

    [Fact]
    public void GivenDiSetup_WhenTestsBegin_ThenSutNotNull()
    {
        Assert.NotNull(_sut);
    }

    [Fact]
    public async Task GivenASubRoute_WhenHandleAsyncCalled_ThenRouteIsHit()
    {
        //Route : Root -> Test
        var args = new string[]{ "backtest", "test", "args" };

        await _sut.HandleAsync(args);

        Console.WriteLine("Routes in Root Route");
        foreach(var route in _sut.GetChildRoutes())
        {
            Console.WriteLine(route);
        }
    }
}
