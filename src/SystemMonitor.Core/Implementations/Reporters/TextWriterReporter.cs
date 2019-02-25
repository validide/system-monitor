using System;
using System.IO;
using System.Threading.Tasks;
using SystemMonitor.Core.Contracts.Monitors;
using SystemMonitor.Core.Contracts.Reporters;
using SystemMonitor.Core.Contracts.Serializers;

namespace SystemMonitor.Core.Implementations.Reporters
{
    public class TextWriterReporter<TData> : IReporter<TData>
    {
        private readonly TextWriter _textWriter;
        private readonly ISerializer<TData, string> _serializer;

        public TextWriterReporter(TextWriter textWriter, ISerializer<TData, string> serializer)
        {
            _textWriter = textWriter ?? throw new ArgumentNullException(nameof(textWriter));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async Task ReportAsync(IMonitorResult<TData> data)
        {
            var output = await _serializer.SerializeAsync(data);
            await _textWriter.WriteAsync(output);
        }
    }
}
