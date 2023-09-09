using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace CommonApi.Common.Middlewares;

/// <summary>
/// 请求响应中间件
/// </summary>
public sealed class RequestResponseLoggerMiddleware(IConfiguration config, ILogger<RequestResponseLoggerMiddleware> logger, RequestDelegate next)
{
    /// <summary>
    /// 是否启用记录请求和响应
    /// </summary>
    private readonly bool _isRequestResponseLoggingEnabled = config.GetValue("EnableRequestResponseLogging", false);

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger<RequestResponseLoggerMiddleware> _logger = logger;

    /// <summary>
    /// 可重用MemoryStream
    /// </summary>
    private readonly RecyclableMemoryStreamManager _manager = new();

    /// <summary>
    ///
    /// </summary>
    /// <param name="httpContext"></param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (!_isRequestResponseLoggingEnabled)
        {
            await next(httpContext);
            return;
        }
        // Middleware is enabled only when the EnableRequestResponseLogging config value is set.
        _logger.LogInformation("""
                                                HTTP request information:
                                                Method: {Method}
                                                Path: {Path}
                                                QueryString: {QueryString}
                                                Headers: {Headers}
                                                Schema: {Scheme}
                                                Host: {Host}
                                                Body: {Body}
                                              """, httpContext.Request.Method,
                                                                httpContext.Request.Path,
                                                                httpContext.Request.QueryString,
                                                                httpContext.Request.Headers,
                                                                httpContext.Request.Scheme,
                                                                httpContext.Request.Host,
                                                                await ReadBodyFromRequest(httpContext.Request));

        // Temporarily replace the HttpResponseStream, which is a write-only stream, with a MemoryStream to capture it's value in-flight.
        await using var originalResponseBody = httpContext.Response.Body;
        using var newResponseBody = _manager.GetStream();
        httpContext.Response.Body = newResponseBody;
        // Call the next middleware in the pipeline
        await next(httpContext);
        newResponseBody.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var responseBodyText = await streamReader.ReadToEndAsync();

        _logger.LogInformation("""
                                                HTTP response information:
                                                StatusCode: {StatusCode}
                                                ContentType: {ContentType}
                                                Headers: {Headers}
                                                Body: {Body}
                                             """, httpContext.Response.StatusCode,
                                                            httpContext.Response.ContentType,
                                                            httpContext.Response.Headers,
                                                            responseBodyText);

        newResponseBody.Seek(0, SeekOrigin.Begin);
        await newResponseBody.CopyToAsync(originalResponseBody);
    }

    /// <summary>
    /// 从流中读取请求体
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private static async Task<string> ReadBodyFromRequest(HttpRequest request)
    {
        // Ensure the request's body can be read multiple times (for the next middlewares in the pipeline).
        request.EnableBuffering();

        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await streamReader.ReadToEndAsync();

        // Reset the request's body stream position for next middleware in the pipeline.
        request.Body.Position = 0;
        return requestBody;
    }
}
