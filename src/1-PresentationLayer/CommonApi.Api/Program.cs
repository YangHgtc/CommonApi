using CommonApi.Common.Extensions;
using CommonApi.Common.Middlewares;
using Serilog;

SerilogExtension.CreateBootstrapLogger();

try
{
    Log.Information("Web application is starting");
    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration;
    builder.Host.AddSerilog();
    // Add services to the container.
    builder.Services.AddMvcControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    if (builder.Environment.IsDevelopment())
    {
#if UseSwagger
        builder.Services.AddSwagger();
#endif
    }

    builder.Services.AddServices(config);
    builder.Services.AddMyCors();
    var app = builder.Build();
    Log.Information(app.Environment.EnvironmentName);
    if (app.Environment.IsDevelopment())
    {
        app.UseMySwagger();
    }
    app.UseSerilogRequestLogging();
    //app.UseMiddleware<RequestResponseLoggerMiddleware>();// 请求响应中间件一定要在异常处理中间件的前面，不然会导致流无法处理
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseRouting();
    app.UseCors("AllowAllOrigins");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
