using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CommonApi.Common.Extensions;

public static class SwaggerExtension
{
    /// <summary>
    /// 添加swagger配置
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // Register the Swagger generator, defining 1 or more Swagger documents
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Common API",
                Version = "v1",
                Description = "An API to perform Common operations",
                //TermsOfService = new Uri("https://learn.microsoft.com/en-us/dotnet/csharp/"),
                Contact = new OpenApiContact
                {
                    Name = "yzc",
                    Email = "xxxx@gmail.com",
                    //Url = new Uri("https://learn.microsoft.com/en-us/dotnet/csharp/"),
                },
                License = new OpenApiLicense
                {
                    Name = "xxxx",
                    //Url = new Uri("https://learn.microsoft.com/en-us/dotnet/csharp/"),
                }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = """
                      JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'
                      """,
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                        {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
            var currentDomain = AppDomain.CurrentDomain;

            // Get all assemblies loaded in the current application domain
            var assemblies = currentDomain.GetAssemblies();

            // Loop through each assembly and print its full name
            var mainAssemblyName = string.Empty;
            foreach (var assembly in assemblies)
            {
                var currentAssemblyName = assembly.GetName().Name;
                if (currentAssemblyName.EndsWith("api", StringComparison.OrdinalIgnoreCase))
                {
                    mainAssemblyName = currentAssemblyName;
                    break;
                }
            }
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{mainAssemblyName}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        return services;
    }
}
