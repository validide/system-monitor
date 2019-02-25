using System;
using SystemMonitor.Core.Contracts.Monitors;

namespace SystemMonitor.Core.Implementations.Monitors
{
    public class MonitorResult<TData> : IMonitorResult<TData>
    {
        public MonitorResult()
        {
            Created = DateTime.UtcNow.ToBinary();
        }

        public long Created { get; set; }
        public TData Value { get; set; }
    }
}
