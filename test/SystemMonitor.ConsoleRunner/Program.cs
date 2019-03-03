using System;
using System.Threading.Tasks;
using SystemMonitor.Core.Implementations.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using SystemMonitor.Core.Implementations;
using SystemMonitor.Core.Implementations.Monitors;
using SystemMonitor.Core.Contracts.ExtensionMethods;
using SystemMonitor.Core.Implementations.Reporters;
using SystemMonitor.Core.Implementations.Serializers;

namespace SystemMonitor.ConsoleRunner
{
    class Program
    {
        public static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        public async static Task MainAsync(string[] args)
        {
            var tcs = new TaskCompletionSource<object>();
            Console.CancelKeyPress += (sender, e) => { e.Cancel = true; tcs.SetResult(null); };

            var host = new Host(
                "Test Console Runner",
                new[] {
                    new SystemMonitorPipeline<string>(
                        new SimpleMonitor<string>(() => new MonitorResult<string>
                        {
                            Value = $"{DateTime.UtcNow:HH:mm:ss} Hello!"
                        }.AsIMonitorResult()),
                        new TextWriterReporter<string>(Console.Out, new TextSerializer<string>(" "))
                    )
                },
                new TimeSpan(0, 0, 5),
                NullLoggerFactory.Instance
            );
            Console.Title = host.Name;

            await host.StartAsync();
            await Console.Out.WriteLineAsync("Press Ctrl+C to exit...");

            await tcs.Task;
            await host.StopAsync();
        }
    }
}
