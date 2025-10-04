using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CliRouter.Core.Extensions;
using CliRouter.Core.Routes;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services
            .AddCliRouting();
    })
    .Build();

var router = host.Services.GetRequiredService<RootRoute>();

await router.HandleAsync(args);
