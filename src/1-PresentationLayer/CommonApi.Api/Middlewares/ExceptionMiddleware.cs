using System.Net;
using System.Text.Json;
using CommonApi.Util;
using Serilog;

namespace CommonApi.Api.Middlewares;

public sealed class MyExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }
            var errorResult = new ErrorResult();
            if (exception is FluentValidation.ValidationException fluentException)
            {
                foreach (var error in fluentException.Errors)
                {
                    errorResult.Messages.Add(error.ErrorMessage);
                }
            }

            errorResult.StatusCode = exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                FluentValidation.ValidationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError,
            };
            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.StatusCode;
                await response.WriteAsync(JsonSerializer.Serialize(errorResult, JsonHelper.jsonSerializerOptions));
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started.");
            }
        }
    }

    public sealed class ErrorResult
    {
        public int StatusCode { get; set; }

        public List<string> Messages { get; set; } = new();
    }
}
