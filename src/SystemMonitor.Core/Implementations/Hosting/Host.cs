using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemMonitor.Core.Contracts;

namespace SystemMonitor.Core.Implementations.Hosting
{
    public class Host
    {
        public readonly string Name;
        private readonly ISystemMonitorPipeline[] _systemMonitorPipelines;
        private readonly Timer _timer;
        private readonly TimeSpan _period;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<Host> _logger;

        public Host(string name, ISystemMonitorPipeline[] systemMonitorPipelines, TimeSpan period, ILoggerFactory loggerFactory)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _systemMonitorPipelines = systemMonitorPipelines ?? throw new ArgumentNullException(nameof(systemMonitorPipelines));
            _period = period;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<Host>();
            _timer = new Timer(RunTasks, null, Timeout.Infinite, Timeout.Infinite);
        }

        public Task StartAsync()
        {
            try
            {
                _timer.Change(TimeSpan.Zero, _period);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"{Name} failed to start.");
            }
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            try
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"{Name} failed to stop correctly.");
            }
            return Task.CompletedTask;
        }

        private void RunTasks(object state)
        {
            _ = RunTasksAsync(state);
        }

        private async Task RunTasksAsync(object state)
        {
            try
            {
                foreach (var pipeline in _systemMonitorPipelines)
                {
                    await pipeline.RunAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Name}.{nameof(RunTasksAsync)} failed.");
            }
        }
    }
}
