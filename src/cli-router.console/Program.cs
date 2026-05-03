using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CliRouter.Core.Extensions;
using CliRouter.Core.Routes;
using CliRouter.Core.Orchestrators;

using System.Reflection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services
            .AddCliRouting();
    })
    .Build();

var router = host.Services.GetRequiredService<RouteOrchestrator>();

/* 
   Handle the commands:
   new 
   - file
   - directory

   delete 
   - file <name>
   - directory <name>

   list
*/
await router.HandleAsync(args);
