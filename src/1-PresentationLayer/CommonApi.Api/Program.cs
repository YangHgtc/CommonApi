using CommonApi.Common.Extensions;
using CommonApi.Common.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Host.AddSerilog();

// Add services to the container.
builder.Services.AddJsonSetting();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddServices(config);
builder.Services.AddMyCors();
var app = builder.Build();
Log.Information(app.Environment.EnvironmentName);
if (app.Environment.IsDevelopment())
{
    app.UseMySwaager();
}
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RequestResponseLoggerMiddleware>();
app.MapControllers();
app.Run();
