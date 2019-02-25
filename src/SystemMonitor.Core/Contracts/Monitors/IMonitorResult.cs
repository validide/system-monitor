namespace SystemMonitor.Core.Contracts.Monitors
{
    public interface IMonitorResult<out TData>
    {
        long Created { get; }
        TData Value { get; }
    }
}
