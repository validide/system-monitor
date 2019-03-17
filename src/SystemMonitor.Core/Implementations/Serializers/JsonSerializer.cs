using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using SystemMonitor.Core.Contracts.Monitors;
using SystemMonitor.Core.Contracts.Serializers;

namespace SystemMonitor.Core.Implementations.Serializers
{
    public class JsonSerializer<TInput> : ISerializer<TInput, string>
    {
        private static Task<string> StringEmptyTask;

        static  JsonSerializer()
        {
            StringEmptyTask = Task.FromResult(String.Empty);
        }

        public Task<string> SerializeAsync(IMonitorResult<TInput> data)
        {
            if (data == null)
                return StringEmptyTask;

            using (var ms = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(
                    typeof(IMonitorResult<TInput>),
                    new[] { data.GetType() }
                );
                serializer.WriteObject(ms, data);
                var json = ms.ToArray();
                var result = Encoding.UTF8.GetString(json, 0, json.Length);
                return Task.FromResult(result);
            }            
        }
    }
}
