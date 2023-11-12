using CommonApi.Util.Helpers;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace CommonApi.Common.Extensions;

/// <summary>
/// </summary>
public static class SerilogExtension
{
    /// <summary>
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IHostBuilder AddSerilog(this IHostBuilder builder)
    {
        builder.UseSerilog((context, services, configuration) => configuration
                            .ReadFrom.Configuration(context.Configuration)
                            .ReadFrom.Services(services)
                            .Enrich.FromLogContext());
        return builder;
    }

    /// <summary>
    /// 创建两段初始化
    /// </summary>
    public static void CreateBootstrapLogger()
    {
        var configuration = ConfigurationHelper.Instance;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }
}
