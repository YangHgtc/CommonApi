using CommonApi.Util.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CommonApi.Common.Filters;

/// <summary>
/// 记录请求过滤器
/// </summary>
/// <param name="logger"></param>
public sealed class RequestLoggerFilter(ILogger<RequestLoggerFilter> logger) : IActionFilter
{
    /// <inheritdoc/>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        logger.LogInformation("""
                                HTTP request information:
                                Method: {Method}
                                Path: {Path}
                                QueryString: {QueryString}
                                Headers: {Headers}
                                Schema: {Scheme}
                                Host: {Host}
                                Body: {Body}
                              """, context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            context.HttpContext.Request.QueryString,
            context.HttpContext.Request.Headers,
            context.HttpContext.Request.Scheme,
            context.HttpContext.Request.Host,
            context.ActionArguments.Values.Serialize());
    }

    /// <inheritdoc/>
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}
