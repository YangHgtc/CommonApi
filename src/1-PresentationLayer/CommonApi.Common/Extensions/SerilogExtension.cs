using Microsoft.Extensions.Hosting;
using Serilog;

namespace CommonApi.Common.Extensions;

public static class SerilogExtension
{
    public static IHostBuilder AddSerilog(this IHostBuilder builder)
    {
        builder.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());
        return builder;
    }
}
