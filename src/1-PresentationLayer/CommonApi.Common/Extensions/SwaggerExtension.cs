using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CommonApi.Common.Extensions;

/// <summary>
/// swagger扩展
/// </summary>
public static class SwaggerExtension
{
    /// <summary>
    /// 添加swagger配置
    /// </summary>
    /// <param name="services"> </param>
    /// <returns> </returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // Register the Swagger generator, defining 1 or more Swagger documents
        services.AddSwaggerGen(c =>
        {
            AddSwaggerDoc(c);
            AddSecurityDefinition(c);
            AddSecurityRequirement(c);
            AddXmlComments(c);
        });

        return services;
    }

    /// <summary>
    /// 使用swagger
    /// </summary>
    /// <param name="app"> </param>
    /// <returns> </returns>
    public static WebApplication UseMySwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    /// <summary>
    /// AddSecurityDefinition
    /// </summary>
    /// <param name="c"> </param>
    private static void AddSecurityDefinition(SwaggerGenOptions c)
    {
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
    }

    /// <summary>
    /// AddSecurityRequirement
    /// </summary>
    /// <param name="c"> </param>
    private static void AddSecurityRequirement(SwaggerGenOptions c)
    {
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                Array.Empty<string>()
            }
        });
    }

    /// <summary>
    /// </summary>
    /// <param name="c"> </param>
    private static void AddSwaggerDoc(SwaggerGenOptions c)
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
                Email = "xxxx@gmail.com"
                //Url = new Uri("https://learn.microsoft.com/en-us/dotnet/csharp/"),
            },
            License = new OpenApiLicense
            {
                Name = "xxxx"
                //Url = new Uri("https://learn.microsoft.com/en-us/dotnet/csharp/"),
            }
        });
    }

    /// <summary>
    /// 添加xml注释文档
    /// </summary>
    /// <param name="c"> </param>
    private static void AddXmlComments(SwaggerGenOptions c)
    {
        var xmlPath = Path.Combine(AppContext.BaseDirectory, "Documentation");
        var xmlFiles = Directory.GetFiles(xmlPath, "*.xml");
        foreach (var item in xmlFiles)
        {
            c.IncludeXmlComments(item, true);
        }
    }
}
