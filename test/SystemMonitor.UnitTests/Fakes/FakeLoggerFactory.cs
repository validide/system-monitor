using Microsoft.Extensions.Logging;

namespace SystemMonitor.UnitTests.Fakes
{
    public class FakeLoggerFactory : ILoggerFactory
    {
        private readonly ILogger _logger;
        public FakeLoggerFactory(ILogger logger)
        {
            _logger = logger;
        }
        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger;
        }

        public void Dispose()
        {
        }
    }
}
