using System;
using System.Threading.Tasks;
using SystemMonitor.Core.Implementations.Monitors;
using Xunit;

namespace SystemMonitor.UnitTests.Core.Monitors
{
    public class SystemDiagnosticsMonitorTests
    {
        [Fact]
        public async Task GetDataAsync_Basic()
        {
            var data = await new SystemDiagnosticsMonitor().GetDataAsync();

            Assert.Equal(Environment.MachineName, data.Value.MachineName);
            Assert.Equal(Environment.Is64BitOperatingSystem, data.Value.Is64BitOperatingSystem);
            Assert.Equal(Environment.ProcessorCount, data.Value.ProcessorCount);
            Assert.Equal(Environment.OSVersion.Platform, data.Value.PlatformId);
            Assert.Equal(Environment.OSVersion.ServicePack, data.Value.ServicePack);
            Assert.Equal(Environment.OSVersion.Version.ToString(), data.Value.Version);
        }
    }
}
