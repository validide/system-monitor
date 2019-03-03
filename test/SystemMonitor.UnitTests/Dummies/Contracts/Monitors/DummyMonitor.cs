using SystemMonitor.Core.Contracts.ExtensionMethods;
using SystemMonitor.Core.Implementations.Monitors;

namespace SystemMonitor.UnitTests.Dummies.Core.Contracts.Monitors
{
    public class DummyMonitor : SimpleMonitor<bool>
    {
        public DummyMonitor() : base(() => new MonitorResult<bool> { Value = false }.AsIMonitorResult())
        {
        }
    }
}
