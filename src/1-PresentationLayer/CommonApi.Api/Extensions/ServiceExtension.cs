using CommonApi.Business;
using CommonApi.Repository;
using CommonApi.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace CommonApi.Api.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// 注入所需服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddMiddlerwares()
                    .AddRepository()
                    .AddBusiness()
                    .AddValidation();
            return services;
        }

        /// <summary>
        /// 注入中间件
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMiddlerwares(this IServiceCollection services)
        {
            services.Scan(scan =>
            {
                scan.FromAssemblyOf<Program>()
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
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ValidationForScrutor>()
                    .AddFluentValidationAutoValidation();
            return services;
        }

        /// <summary>
        /// 注入business
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusiness(this IServiceCollection services)
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
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.Scan(scan =>
            {
                scan.FromAssemblyOf<RepositoryForScrutor>()
                .AddClasses()
                .AsMatchingInterface()
                .WithScopedLifetime();
            });
            return services;
        }
    }
}
