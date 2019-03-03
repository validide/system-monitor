using System.Threading.Tasks;

namespace SystemMonitor.Core.Contracts
{
    public interface ISystemMonitorPipeline
    {
        Task RunAsync();
    }
}
