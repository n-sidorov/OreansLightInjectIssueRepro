using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using SimpleGrain;

namespace Host
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ILoggerFactory logger = new LoggerFactory();
            logger.AddConsole(minLevel: LogLevel.Debug);

            var lightInjectLogger = logger.CreateLogger("LightInject");

            var opts = new LightInject.ContainerOptions
            {
                EnablePropertyInjection = false,
                LogFactory = _ => (msg) => lightInjectLogger.LogDebug(msg.Message)
            };
            var container = new LightInject.ServiceContainer(opts);
            container.RegisterInstance(logger);

            var siloBuilder = new SiloHostBuilder()
                .UseServiceProviderFactory(services =>
                {
                    return container.CreateServiceProvider(services);
                })
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "dev";
                })
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences();
                });

            var host = siloBuilder.Build();

            await host.StartAsync();
            Console.WriteLine("Press Enter to terminate...");
            Console.ReadLine();
            await host.StopAsync();
        }
    }
}
