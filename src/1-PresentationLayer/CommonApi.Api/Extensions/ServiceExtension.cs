using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace CommonApi.Api.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddRepository();
            services.AddBusiness();
            services.AddValidation();
            return services;
        }

        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            var validation = Assembly.Load("CommonApi.Validation");
            services.AddValidatorsFromAssembly(validation)
                    .AddFluentValidationAutoValidation();
            return services;
        }

        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            var business = Assembly.Load("CommonApi.Business");
            services.Scan(scan =>
            {
                scan.FromAssemblies(business)
                .AddClasses()
                .AsMatchingInterface()
                .WithScopedLifetime();
            });
            return services;
        }

        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            var repository = Assembly.Load("CommonApi.Repository");
            services.Scan(scan =>
            {
                scan.FromAssemblies(repository)
                .AddClasses()
                .AsMatchingInterface()
                .WithScopedLifetime();
            });
            return services;
        }
    }
}
