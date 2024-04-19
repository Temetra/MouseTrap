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
        var level = LogEventLevel.Debug;
#else
        var level = LogEventLevel.Information;
#endif

        return new LoggerConfiguration()
            .MinimumLevel.Is(level)
            .WriteTo.File(
                path: "log.txt",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 2)
            .CreateLogger();
    }
}
