using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace MouseTrap.Core;

public class Log
{
    public static readonly ILogger Logger = CreateLogger();

    private static Logger CreateLogger()
    {
#if DEBUG
        var cfg = new LoggerConfiguration()
            .MinimumLevel.Is(LogEventLevel.Debug)
            .WriteTo.File($"log-{DateTime.Now:yyyyMMdd-HHmmss}.txt");
#else
        var cfg = new LoggerConfiguration()
            .MinimumLevel.Is(LogEventLevel.Information)
            .WriteTo.File(
                path: $"log.txt",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 2);
#endif
        return cfg.CreateLogger();
    }
}
