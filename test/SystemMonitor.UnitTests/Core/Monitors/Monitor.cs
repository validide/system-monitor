using System;
using System.Threading.Tasks;
using SystemMonitor.Core.Implementations.Monitors;
using SystemMonitor.UnitTests.Dummies.Core.Contracts.Monitors;
using Xunit;

namespace SystemMonitor.UnitTests.Core.Monitor
{
    public class Monitor
    {
        [Fact]
        public async Task GetDataAsync_NotNull()
        {
            var mon = new DummyMonitor();
            var data = await mon.GetDataAsync();
            Assert.NotNull(data);
        }

        [Fact]
        public async Task GetDataAsync_FirstIsZero()
        {
            var mon = new DummyMonitor();
            var data = await mon.GetDataAsync();
            Assert.False(data.Value);
        }

        [Fact]
        public void SimpleMonitor_NullFunction()
        {
            Assert.Throws<ArgumentNullException>(() => { _ = new SimpleMonitor<object>(null); });
        }
    }
}
