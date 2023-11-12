using CommonApi.Common.Extensions;
using CommonApi.Util.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CommonApi.Common.Filters;

/// <summary>
/// 记录响应过滤器
/// </summary>
/// <param name="logger"> </param>
public sealed class ResponseLoggerFilter(ILogger<RequestLoggerFilter> logger) : IResultFilter
{
    /// <inheritdoc />
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context?.Result is ObjectResult result)
        {
            //统一包装400序列化失败请求
            if (result is { StatusCode: StatusCodes.Status400BadRequest, Value: ValidationProblemDetails detail })
            {
                var errors = detail.Errors.Where(x => x.Key != "$").SelectMany(x => x.Value);
                var message = string.Join(';', errors);
                context.Result = new BadRequestObjectResult(message);
            }

            var response = result.Value;
            logger.LogResponse(
                context.HttpContext.Request.Path,
                context.HttpContext.Response.StatusCode,
                context.HttpContext.Response.ContentType,
                context.HttpContext.Response.Headers,
                response.Serialize());
        }
    }

    /// <inheritdoc />
    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}
