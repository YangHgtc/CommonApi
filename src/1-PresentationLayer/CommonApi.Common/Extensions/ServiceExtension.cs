using CommonApi.Business;
using CommonApi.Common.Common;
using CommonApi.DataBase;
using CommonApi.DataBase.Dapper;
using CommonApi.Entity;
using CommonApi.Mapper;
using CommonApi.Repository;
using CommonApi.Validation;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonApi.Common.Extensions;

public static class ServiceExtension
{
    /// <summary>
    /// 注入所需服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration config)
    {
        services.AddMiddlewares()
                .AddRepository()
                .AddBusiness()
                .AddValidation()
                .AddMapper()
                .AddJwt(config);
        return services;
    }

    /// <summary>
    /// 注入中间件
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.Scan(scan =>
        {
            scan.FromAssemblyOf<ApiControllerBase>()
            .AddClasses(x => x.Where(y => y.Namespace!.Contains("Middlewares", StringComparison.OrdinalIgnoreCase)))
            .AsSelf()
            .WithSingletonLifetime();
        });
        return services;
    }

    /// <summary>
    /// 注入验证规则
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ValidationForScrutor>();
        //.AddFluentValidationAutoValidation();
        return services;
    }

    /// <summary>
    /// 注入business
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.Scan(scan =>
        {
            scan.FromAssemblyOf<BusinessForScrutor>()
            .AddClasses()
            .AsMatchingInterface()
            .WithScopedLifetime();
        });
        return services;
    }

    /// <summary>
    /// 注入仓储
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.Scan(scan =>
        {
            scan.FromAssemblyOf<RepositoryForInjection>()
            .AddClasses()
            .AsMatchingInterface()
            .WithScopedLifetime();
        });
        return services;
    }
    
    /// <summary>
    /// 注册jwt
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    private static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration config)
    {
        var jwtOption = config.GetSection(JwtOptions.Position);
        ArgumentNullException.ThrowIfNull(jwtOption, nameof(jwtOption));
        services.AddOptions<JwtOptions>().Bind(jwtOption);
        services.AddScoped<IJwtService, JwtService>();
        services.AddJwtAuthentication(jwtOption.Get<JwtOptions>());
        return services;
    }
    
    /// <summary>
    /// 注册dapper
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddDapper(this IServiceCollection services, ConfigurationManager config)
    {
        var connection = config.GetValue<string>("ConnectionStrings:DefaultConnection");
        ArgumentNullException.ThrowIfNull(connection, nameof(config));
        services.AddSingleton<IDbConnectionFactory>(_ => new MysqlConnectionFactory(connection));
        services.AddSingleton<IDapperHelperAsync, DapperHelperAsync>();
        //添加dapper实体类
        var assembly = typeof(EntityForInjection).Assembly;
        ColumnMapper.FindCustomAttributesPropertyInfo(assembly, assembly.GetName().Name);
        return services;
    }
    
    /// <summary>
    /// 注册mapper
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        // scans the assembly and gets the IRegister, adding the registration to the TypeAdapterConfig
        typeAdapterConfig.Scan(typeof(MapperForInjection).Assembly);
        // register the mapper as Singleton service for my application
        var mapperConfig = new MapsterMapper.Mapper(typeAdapterConfig);
        services.AddSingleton<IMapper>(mapperConfig);
        return services;
    }
}
