using System.Threading.Tasks;
using SystemMonitor.Core.Contracts.Monitors;

namespace SystemMonitor.Core.Contracts.Serializers
{
    public interface ISerializer<TInput, TOutput>
    {
        Task<TOutput> SerializeAsync(IMonitorResult<TInput> data);
    }
}
