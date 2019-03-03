using System.Threading.Tasks;

namespace SystemMonitor.Core.Contracts.Hosting
{
    public interface IHost
    {
        Task StartAsync();
        Task StopAsync();
    }
}
