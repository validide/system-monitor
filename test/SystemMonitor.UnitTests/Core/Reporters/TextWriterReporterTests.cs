using System;
using System.IO;
using System.Threading.Tasks;
using SystemMonitor.Core.Implementations.Monitors;
using SystemMonitor.Core.Implementations.Reporters;
using SystemMonitor.Core.Implementations.Serializers;
using Xunit;

namespace SystemMonitor.UnitTests.Core.Monitor
{
    public class TextWriterReporterTests
    {
        [Fact]
        public void Constructor_NullTextWriter()
        {
            Assert.Throws<ArgumentNullException>(() => { _ = new TextWriterReporter<object>(null, new TextSerializer<object>("\t")); });
        }

        [Fact]
        public void Constructor_NullSerializer()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                using (var sw = new StringWriter())
                    _ = new TextWriterReporter<object>(sw, null);
            });
        }

        [Fact]
        public async Task TextWriterReporter_ReportAsync()
        {
            using (var sw = new StringWriter())
            {
                var data = new MonitorResult<object> { Value = null };
                var serializer = new TextSerializer<object>("\t");
                var reporter = new TextWriterReporter<object>(sw, serializer);

                var serializedData = await serializer.SerializeAsync(data);
                await reporter.ReportAsync(data);
                Assert.Equal(serializedData, sw.ToString());
            }
                
        }
    }
}
