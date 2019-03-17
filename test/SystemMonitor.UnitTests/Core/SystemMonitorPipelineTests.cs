using System;
using System.IO;
using System.Threading.Tasks;
using SystemMonitor.Core.Implementations;
using SystemMonitor.Core.Implementations.Reporters;
using SystemMonitor.Core.Implementations.Serializers;
using SystemMonitor.UnitTests.Dummies.Core.Contracts.Monitors;
using Xunit;

namespace SystemMonitor.UnitTests.Core
{
    public class SystemMonitorPipelineTests
    {
        [Fact]
        public void SystemMonitorPipeline_NullMonitor()
        {
            Assert.Throws<ArgumentNullException>(() => 
            {
                using (var sw = new StringWriter())
                {
                    _ = new SystemMonitorPipeline<object>(
                        null,
                        new TextWriterReporter<object>(sw, new TextSerializer<object>(" "))
                    );
                }                    
            });
        }

        [Fact]
        public void SystemMonitorPipeline_NullReporter()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new SystemMonitorPipeline<bool>(
                    new DummyMonitor(),
                    null
                );
            });
        }

        [Fact]
        public async Task GetRunAsync()
        {
            using (var sw = new StringWriter())
            {
                var monitor = new DummyConstantMonitor();
                var serializer = new TextSerializer<bool>(" ");
                var reporter = new TextWriterReporter<bool>(sw, serializer);
                var pipeline = new SystemMonitorPipeline<bool>(monitor, reporter);

                var serializedData = await serializer.SerializeAsync(await monitor.GetDataAsync());
                await pipeline.RunAsync();

                Assert.Equal($"{serializedData}{Environment.NewLine}", sw.ToString());
            }
        }
    }
}
