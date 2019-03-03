using System;
using System.Threading.Tasks;
using SystemMonitor.Core.Contracts;
using SystemMonitor.Core.Contracts.Monitors;
using SystemMonitor.Core.Contracts.Reporters;

namespace SystemMonitor.Core.Implementations
{
    public class SystemMonitorPipeline<TData> : ISystemMonitorPipeline
    {
        private readonly IMonitor<TData> _monitor;
        private readonly IReporter<TData> _reporter;

        public SystemMonitorPipeline(IMonitor<TData> monitor, IReporter<TData> reporter)
        {
            _monitor = monitor ?? throw new ArgumentNullException(nameof(monitor));
            _reporter = reporter ?? throw new ArgumentNullException(nameof(reporter));
        }

        public async Task RunAsync()
        {
            var moitorData = await _monitor.GetDataAsync();
            await _reporter.ReportAsync(moitorData);
        }
    }
}
