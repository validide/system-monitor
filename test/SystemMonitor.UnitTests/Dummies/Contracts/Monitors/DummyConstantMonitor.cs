using SystemMonitor.Core.Contracts.ExtensionMethods;
using SystemMonitor.Core.Contracts.Monitors;
using SystemMonitor.Core.Implementations.Monitors;

namespace SystemMonitor.UnitTests.Dummies.Core.Contracts.Monitors
{
    public class DummyConstantMonitor : SimpleMonitor<bool>
    {
        private static IMonitorResult<bool> _monitorResult = new MonitorResult<bool> { Value = false }.AsIMonitorResult();

        public DummyConstantMonitor() : base(() => _monitorResult)
        {
        }
    }
}
