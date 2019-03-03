using System;
using System.Threading.Tasks;
using SystemMonitor.Core.Contracts.Monitors;

namespace SystemMonitor.Core.Implementations.Monitors
{
    public class SimpleMonitor<T> : IMonitor<T>
    {
        private readonly Func<IMonitorResult<T>> _monitorFunc;

        public SimpleMonitor(Func<IMonitorResult<T>> monitorFunc)
        {
            _monitorFunc = monitorFunc ?? throw new ArgumentNullException(nameof(monitorFunc));
        }

        public Task<IMonitorResult<T>> GetDataAsync()
        {
            return Task.FromResult(_monitorFunc.Invoke());
        }
    }
}
