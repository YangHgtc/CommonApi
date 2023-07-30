using Microsoft.Extensions.DependencyInjection;

namespace CommonApi.Common.Extensions;

/// <summary>
/// 依赖注入扩展
/// </summary>
public static class InjectionExtension
{
    /// <summary>
    /// 通过扫描程序集进行注册
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="serviceLifetime">生命周期</param>
    /// <returns></returns>
    public static IServiceCollection RegisterByScanAssembly<T>(this IServiceCollection services, ServiceLifetime serviceLifetime)
    {
        services.Scan(scan =>
        {
            scan.FromAssemblyOf<T>()
                .AddClasses()
                .AsMatchingInterface()
                .WithLifetime(serviceLifetime);
        });
        return services;
    }

    /// <summary>
    /// 注册为scope
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterScopedByScanAssembly<T>(this IServiceCollection services)
    {
        return services.RegisterByScanAssembly<T>(ServiceLifetime.Scoped);
    }

    /// <summary>
    /// 注册为Singleton
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterSingletonByScanAssembly<T>(this IServiceCollection services)
    {
        return services.RegisterByScanAssembly<T>(ServiceLifetime.Singleton);
    }

    /// <summary>
    /// 注册为Transient
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterTransientByScanAssembly<T>(this IServiceCollection services)
    {
        return services.RegisterByScanAssembly<T>(ServiceLifetime.Transient);
    }
}
