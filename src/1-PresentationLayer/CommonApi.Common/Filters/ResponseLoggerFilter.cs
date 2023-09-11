using CommonApi.Util.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CommonApi.Common.Filters;

/// <summary>
/// 记录响应过滤器
/// </summary>
/// <param name="logger"></param>
public sealed class ResponseLoggerFilter(ILogger<RequestLoggerFilter> logger) : IResultFilter
{
    /// <inheritdoc/>
    public void OnResultExecuting(ResultExecutingContext context)
    {
    }

    /// <inheritdoc/>
    public void OnResultExecuted(ResultExecutedContext context)
    {
        if (context?.Result is ObjectResult result)
        {
            var response = result.Value;
            logger.LogInformation("""
                                     HTTP response information:
                                     StatusCode: {StatusCode}
                                     ContentType: {ContentType}
                                     Headers: {Headers}
                                     Body: {Body}
                                  """, context.HttpContext.Response.StatusCode,
                context.HttpContext.Response.ContentType,
                context.HttpContext.Response.Headers,
                response.Serialize());
        }
    }
}
