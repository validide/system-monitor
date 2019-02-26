using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemMonitor.Core.Contracts.Monitors;
using SystemMonitor.Core.Contracts.Serializers;

namespace SystemMonitor.Core.Implementations.Serializers
{
    public class TextSerializer<TInput> : ISerializer<TInput, string>
    {
        protected readonly string Delimiter;
        protected readonly bool IsValueType;
        protected readonly bool IsStringType;

        public TextSerializer(string delimiter)
        {
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            if (delimiter.Length == 0) throw new ArgumentOutOfRangeException(nameof(delimiter));
            Delimiter = delimiter;
            IsValueType = typeof(TInput).IsValueType;
            IsStringType = typeof(TInput) == typeof(string);
        }

        public async virtual Task<string> SerializeAsync(IMonitorResult<TInput> data)
        {
            var sb = new StringBuilder();
            sb.Append(data.Created);
            if (data.Value != null)
            {
                await SerializeDataAsync(sb, data.Value);
            }
            return sb.ToString();
        }

        protected virtual Task SerializeDataAsync(StringBuilder stringBuilder, TInput data)
        {
            if (IsValueType || IsStringType)
            {
                stringBuilder.Append(Delimiter);
                stringBuilder.Append(data);
            }
            else
            {
                foreach (var cellValue in GetCellValues(data))
                {
                    stringBuilder.Append(Delimiter);
                    AppendCellValue(stringBuilder, cellValue);
                }

            }
            return Task.CompletedTask;
        }

        protected virtual void AppendCellValue(StringBuilder stringBuilder, string cellValue)
        {
            var mustQuote = cellValue != null
                            && (cellValue.Contains(Delimiter)
                            || cellValue.Contains("\"")
                            || cellValue.Contains("\r")
                            || cellValue.Contains("\n"));

            if (mustQuote)
            {
                stringBuilder.Append('\"');
                foreach (char nextChar in cellValue)
                {
                    stringBuilder.Append(nextChar);
                    if (nextChar == '"')
                    {
                        stringBuilder.Append('\"');
                    }
                }
                stringBuilder.Append('\"');
            }
            else
            {
                stringBuilder.Append(cellValue);
            }
        }

        protected virtual IEnumerable<string> GetCellValues(TInput data)
        {
            foreach (var property in typeof(TInput).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                yield return property.GetValue(data)?.ToString();
            }
        }
    }
}
