using System.Threading.Tasks;
using SystemMonitor.Core.Contracts.ExtensionMethods;
using SystemMonitor.Core.Contracts.Monitors;
using SystemMonitor.Core.Implementations.Monitors;

namespace SystemMonitor.UnitTests.Dummies.Core.Contracts.Monitors
{
    public class DummyMonitor : IMonitor<bool>
    {
        public Task<IMonitorResult<bool>> GetDataAsync()
        {
            return Task.FromResult(new MonitorResult<bool> { Value = false }.AsIMonitorResult());
        }
    }
}
