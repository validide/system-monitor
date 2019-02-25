using System.Threading.Tasks;
using SystemMonitor.Core.Contracts.Monitors;

namespace SystemMonitor.Core.Contracts.Reporters
{
    public interface IReporter<TData>
    {
        Task ReportAsync(IMonitorResult<TData> data);
    }
}
