using System;

namespace SystemMonitor.Core.Implementations.Monitors
{
    public class SystemDiagnosticsMonitorResult
    {
        public string MachineName { get; set; }
        public bool Is64BitOperatingSystem { get; set; }
        public int ProcessorCount { get; set; }
        public PlatformID PlatformId { get; set; }
        public string ServicePack { get; set; }
        public string Version { get; set; }
    }
}
