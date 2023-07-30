using System.Net;
using CommonApi.Common.Common;
using CommonApi.Util.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CommonApi.Common.Middlewares;

/// <summary>
/// 异常处理中间件
/// </summary>
public sealed class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "发生了异常");
            await HandleException(context, exception);
        }
    }

    /// <summary>
    /// 处理异常
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private async Task HandleException(HttpContext context, Exception exception)
    {
        if (exception.InnerException != null)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
        }

        var errorResult = new ResponseResult<bool>() { Code = (int)ResponseStatusCode.Fail };
        if (exception is FluentValidation.ValidationException fluentException)
        {
            var errors = fluentException.Errors.Select(error => error.ErrorMessage);
            errorResult.Message = string.Join(";", errors);
        }
        else
        {
            errorResult.Message = exception.Message;
        }

        var response = context.Response;
        response.StatusCode = exception switch
        {
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            FluentValidation.ValidationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError,
        };

        if (!response.HasStarted)
        {
            response.ContentType = "application/json";
            await response.WriteAsync(errorResult.Serialize());
        }
        else
        {
            _logger.LogWarning("Can't write error response. Response has already started.");
        }
    }
}
