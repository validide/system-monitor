using System.Threading.Tasks;

namespace SystemMonitor.Core.Contracts.Monitors
{
    public interface IMonitor<TData>
    {
        Task<IMonitorResult<TData>> GetDataAsync();
    }
}
