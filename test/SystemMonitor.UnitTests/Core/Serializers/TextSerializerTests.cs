using System;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using SystemMonitor.Core.Implementations.Monitors;
using SystemMonitor.Core.Implementations.Serializers;
using Xunit;

namespace SystemMonitor.UnitTests.Core.Monitor
{
    public class TextSerializerTests
    {
        internal class ReportItem
        {
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string E { get; set; }
            public string F { get; set; }
            public string G { get; set; }
        }

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
            const string delim = ",";
            var serializer = new TextSerializer<object>(delim);
            var data = new MonitorResult<object> { Value = null };
            var serialized = await serializer.SerializeAsync(data);
            Assert.Equal($"{data.Created}", serialized);
        }

        [Fact]
        public async Task SerializeAsync_Boolean()
        {
            const string delim = ",";
            var serializer = new TextSerializer<bool>(delim);
            var data = new MonitorResult<bool> { Value = true };
            var serialized = await serializer.SerializeAsync(data);
            Assert.Equal($"{data.Created}{delim}{data.Value}", serialized);
        }

        [Fact]
        public async Task SerializeAsync_NonValueType()
        {
            const string delim = ",";
            var serializer = new TextSerializer<object>(delim);
            var data = new MonitorResult<object> { Value = null };
            var serialized = await serializer.SerializeAsync(data);
            Assert.Equal($"{data.Created}", serialized);
        }

        [Fact]
        public async Task SerializeAsync_StringType()
        {
            const string delim = ",";
            var serializer = new TextSerializer<string>(delim);
            var data = new MonitorResult<string> { Value = Guid.NewGuid().ToString("N") };
            var serialized = await serializer.SerializeAsync(data);
            Assert.Equal($"{data.Created}{delim}{data.Value}", serialized);
        }

        [Fact]
        public async Task SerializeAsync_ComplexTypeType_Null()
        {
            const string delim = ",";
            var serializer = new TextSerializer<ReportItem>(delim);
            var data = new MonitorResult<ReportItem> { Value = null };
            var serialized = await serializer.SerializeAsync(data);
            Assert.Equal($"{data.Created}", serialized);
        }

        [Fact]
        public async Task SerializeAsync_ComplexTypeType_SpecialChars()
        {
            const string delim = " ";
            var serializer = new TextSerializer<ReportItem>(delim);
            var data = new MonitorResult<ReportItem>
            {
                Value = new ReportItem
                {
                    A = Guid.NewGuid().ToString("N"),
                    B = "A" + delim + "B",
                    C = "A\nB",
                    D = "A\rB",
                    E = null,
                    F = "A\"\"B",
                    G = "A\n\"\r\"\"B"
                }
            };
            var serialized = await serializer.SerializeAsync(data);
            var expected = data.Created.ToString();
            expected += $"{delim}{data.Value.A}";
            expected += $"{delim}\"{data.Value.B.Replace("\"", "\"\"")}\"";
            expected += $"{delim}\"{data.Value.C.Replace("\"", "\"\"")}\"";
            expected += $"{delim}\"{data.Value.D.Replace("\"", "\"\"")}\"";
            expected += $"{delim}{data.Value.E}";
            expected += $"{delim}\"{data.Value.F.Replace("\"", "\"\"")}\"";
            expected += $"{delim}\"{data.Value.G.Replace("\"", "\"\"")}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void SerializeAsync_ComplexTypeType_JustForCoverage()
        {
            const string delim = " ";
            var serializer = new TextSerializer<ReportItem>(delim);
            var data = new MonitorResult<ReportItem>
            {
                Value = new ReportItem
                {
                    A = Guid.NewGuid().ToString("N"),
                    B = "A" + delim + "B",
                    C = "A\nB",
                    D = "A\rB",
                    E = null,
                    F = "A\"\"B",
                    G = "A\n\"\r\"\"B"
                }
            };

            var getCellValuesMethod = serializer.GetType().GetMethod("GetCellValues", BindingFlags.Instance | BindingFlags.NonPublic);
            var valuesEnumerator = (getCellValuesMethod.Invoke(serializer, new[] { data.Value }) as IEnumerable).GetEnumerator();
            while (valuesEnumerator.MoveNext())
            {
                Console.WriteLine(valuesEnumerator.Current);
            }
            valuesEnumerator.MoveNext();
        }
    }
}
