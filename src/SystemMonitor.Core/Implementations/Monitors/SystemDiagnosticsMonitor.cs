using System;
using System.Threading.Tasks;
using SystemMonitor.Core.Contracts.ExtensionMethods;
using SystemMonitor.Core.Contracts.Monitors;

namespace SystemMonitor.Core.Implementations.Monitors
{
    public class SystemDiagnosticsMonitor : IMonitor<SystemDiagnosticsMonitorResult>
    {
        public Task<IMonitorResult<SystemDiagnosticsMonitorResult>> GetDataAsync()
        {
            var osVersion = Environment.OSVersion;
            var result = new SystemDiagnosticsMonitorResult
            {
                MachineName = Environment.MachineName,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                ProcessorCount = Environment.ProcessorCount,
                PlatformId = osVersion.Platform,
                ServicePack = osVersion.ServicePack,
                Version = osVersion.Version.ToString()                
            };

            return Task.FromResult(new MonitorResult<SystemDiagnosticsMonitorResult>
            {
                Value = result
            }.AsIMonitorResult());
        }
    }
}
