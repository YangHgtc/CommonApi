using System.Net;
using CommonApi.Common.Common;
using CommonApi.Util.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CommonApi.Common.Middlewares;

/// <summary>
/// 异常处理中间件
/// </summary>
/// <param name="logger"> 日志 </param>
/// <param name="next"> 委托中间件 </param>
/// <remarks>
/// <example> 中间件采用约定的方式编写,具体示例如下
/// <code>
///public sealed class CustomMiddleware(RequestDelegate next)
///{
///public async Task InvokeAsync(HttpContext context)
///{
///await next(context);
///}
///}
/// </code>
/// </example>
/// </remarks>
public sealed class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
{
    /// <summary>
    /// </summary>
    /// <param name="context"> </param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "发生了异常");
            await HandleException(context, exception);
        }
    }

    /// <summary>
    /// 包装异常信息
    /// </summary>
    /// <param name="exception"> </param>
    /// <returns> </returns>
    private static ResponseResult<bool> WrapErrorResult(Exception exception)
    {
        if (exception.InnerException != null)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
        }

        var errorResult = new ResponseResult<bool> { Code = (int)ResponseStatusCode.Fail };
        if (exception is ValidationException fluentException)
        {
            var errors = fluentException.Errors.Select(error => error.ErrorMessage);
            errorResult.Message = string.Join(';', errors);
        }
        else
        {
            errorResult.Message = exception.Message;
        }

        return errorResult;
    }

    /// <summary>
    /// 处理异常
    /// </summary>
    /// <param name="context"> </param>
    /// <param name="exception"> </param>
    /// <returns> </returns>
    private async Task HandleException(HttpContext context, Exception exception)
    {
        var errorResult = WrapErrorResult(exception);

        var response = context.Response;
        response.StatusCode = exception switch
        {
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            ValidationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        if (!response.HasStarted)
        {
            response.ContentType = "application/json";
            await response.WriteAsync(errorResult.Serialize());
        }
        else
        {
            logger.LogWarning("Can't write error response. Response has already started.");
        }
    }
}
