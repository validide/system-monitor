using System;
using System.Threading.Tasks;
using SystemMonitor.Core.Implementations.Monitors;
using SystemMonitor.Core.Implementations.Serializers;
using Xunit;

namespace SystemMonitor.UnitTests.Core.Monitor
{
    public class TextSerializerTests
    {
        [Fact]
        public void Constructor_NullDelimiter()
        {
            Assert.Throws<ArgumentNullException>(() => { _ = new TextSerializer<object>(null); });
        }

        [Fact]
        public void Constructor_EmptyDelimiter()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { _ = new TextSerializer<object>(String.Empty); });
        }

        [Fact]
        public async Task SerializeAsync_Null()
        {
            const string delim = "\t";
            var serializer = new TextSerializer<object>(delim);
            var data = new MonitorResult<object> { Value = null };
            var serialized = await serializer.SerializeAsync(data);
            Assert.Equal($"{data.Created}", serialized);
        }

        [Fact]
        public async Task SerializeAsync_Boolean()
        {
            const string delim = "\t";
            var serializer = new TextSerializer<bool>(delim);
            var data = new MonitorResult<bool> { Value = true };
            var serialized = await serializer.SerializeAsync(data);
            Assert.Equal($"{data.Created}{delim}{data.Value}", serialized);
        }
    }
}
