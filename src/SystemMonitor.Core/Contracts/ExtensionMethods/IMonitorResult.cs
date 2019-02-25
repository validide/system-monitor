using SystemMonitor.Core.Contracts.Monitors;

namespace SystemMonitor.Core.Contracts.ExtensionMethods
{
    public static class IMonitorResult
    {
        public static IMonitorResult<T> AsIMonitorResult<T>(this IMonitorResult<T> item)
        {
            return item;
        }
    }
}
