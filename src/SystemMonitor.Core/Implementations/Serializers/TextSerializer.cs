using System;
using System.Text;
using System.Threading.Tasks;
using SystemMonitor.Core.Contracts.Monitors;
using SystemMonitor.Core.Contracts.Serializers;

namespace SystemMonitor.Core.Implementations.Serializers
{
    public class TextSerializer<TInput> : ISerializer<TInput, string>
    {
        protected readonly string Delimiter;

        public TextSerializer(string delimiter)
        {
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            if (delimiter.Length == 0) throw new ArgumentOutOfRangeException(nameof(delimiter));
            Delimiter = delimiter;
        }

        public async virtual Task<string> SerializeAsync(IMonitorResult<TInput> data)
        {
            var sb = new StringBuilder();
            sb.Append(data.Created);
            if (data.Value != null)
            {
                sb.Append(Delimiter);
                sb.Append(await SerializeDataAsync(data.Value));
            }
            return sb.ToString();
        }

        protected virtual Task<string> SerializeDataAsync(TInput data)
        {
            return Task.FromResult(data.ToString());
        }
    }
}
