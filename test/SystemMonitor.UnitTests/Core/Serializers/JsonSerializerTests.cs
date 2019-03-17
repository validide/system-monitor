using System;
using System.Threading.Tasks;
using SystemMonitor.Core.Implementations.Monitors;
using SystemMonitor.Core.Implementations.Serializers;
using Xunit;

namespace SystemMonitor.UnitTests.Core.Monitor
{
    public class JsonSerializerTests
    {
        [Fact]
        public async Task SerializeAsync_Null()
        {
            var serializer = new JsonSerializer<object>();
            var serialized = await serializer.SerializeAsync(null);
            Assert.Equal(String.Empty, serialized);
        }

        [Fact]
        public async Task SerializeAsync_Boolean()
        {
            var result = new MonitorResult<bool> { Value = true };
            var serialized = await new JsonSerializer<bool>().SerializeAsync(result);
            var expected = "{\"__type\":\"MonitorResultOfboolean:#SystemMonitor.Core.Implementations.Monitors\",\"Created\":"
                + result.Created.ToString()
                + ",\"Value\":true}";
            Assert.Equal(expected, serialized);
        }
    }
}
