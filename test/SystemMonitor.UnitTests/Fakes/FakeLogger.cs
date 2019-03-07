using Microsoft.Extensions.Logging;
using System;

namespace SystemMonitor.UnitTests.Fakes
{
    public class FakeLogger : ILogger
    {
        public Exception ProvidedException { get; private set; }
        public string ProvidedMessage { get; private set; }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            ProvidedException = exception;
            ProvidedMessage = formatter(state, exception);
        }

        public void Reset()
        {
            ProvidedException = null;
            ProvidedMessage = null;
        }
    }
}
