using CommonApi.Common.Extensions;
using CommonApi.Common.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddSerilog();

// Add services to the container.
builder.Services.AddJsonSetting();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddServices();
var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Log.Information(app.Environment.EnvironmentName);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
