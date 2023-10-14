using CommonApi.Common.Extensions;
using CommonApi.Common.Middlewares;
using Serilog;
using Serilog.Events;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
        true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();
try
{
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration;
    builder.Host.AddSerilog();
    // Add services to the container.
    builder.Services.AddMvcControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddSwagger();
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
