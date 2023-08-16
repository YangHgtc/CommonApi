using Microsoft.Extensions.Hosting;
using Serilog;

namespace CommonApi.Common.Extensions;

/// <summary>
/// 
/// </summary>
public static class SerilogExtension
{
    /// <summary>
    /// 
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
}
