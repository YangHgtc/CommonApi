using Microsoft.Extensions.DependencyInjection;

namespace CommonApi.Common.Extensions;

/// <summary>
/// 跨域扩展
/// </summary>
public static class CorsExtension
{
    /// <summary>
    /// 添加跨域
    /// </summary>
    /// <param name="services"> </param>
    /// <returns> </returns>
    public static IServiceCollection AddMyCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        return services;
    }
}
