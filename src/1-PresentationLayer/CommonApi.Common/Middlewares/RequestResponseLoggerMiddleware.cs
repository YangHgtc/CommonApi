using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace CommonApi.Common.Middlewares
{
    /// <summary>
    /// 请求响应中间件
    /// </summary>
    public sealed class RequestResponseLoggerMiddleware : IMiddleware
    {
        /// <summary>
        /// 是否启用记录请求和响应
        /// </summary>
        private readonly bool _isRequestResponseLoggingEnabled;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<RequestResponseLoggerMiddleware> _logger;

        /// <summary>
        /// 可重用MemoryStream
        /// </summary>
        private readonly RecyclableMemoryStreamManager _manager = new();

        public RequestResponseLoggerMiddleware(IConfiguration config, ILogger<RequestResponseLoggerMiddleware> logger)
        {
            _isRequestResponseLoggingEnabled = config.GetValue("EnableRequestResponseLogging", false);
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            if (!_isRequestResponseLoggingEnabled)
            {
                await next(httpContext);
                return;
            }
            // Middleware is enabled only when the EnableRequestResponseLogging config value is set.

            _logger.LogInformation($"""
                                    HTTP request information:
                                    Method: {httpContext.Request.Method}
                                    Path: {httpContext.Request.Path}
                                    QueryString: {httpContext.Request.QueryString}
                                    Headers: {FormatHeaders(httpContext.Request.Headers)}
                                    Schema: {httpContext.Request.Scheme}
                                    Host: {httpContext.Request.Host}
                                    Body: {await ReadBodyFromRequest(httpContext.Request)}
                                """);

            // Temporarily replace the HttpResponseStream, which is a write-only stream, with a MemoryStream to capture it's value in-flight.

            // Call the next middleware in the pipeline

            var originalResponseBody = httpContext.Response.Body;
            using var newResponseBody = _manager.GetStream();
            httpContext.Response.Body = newResponseBody;
            await next(httpContext);
            if (newResponseBody.CanRead && newResponseBody.CanSeek)
            {
                newResponseBody.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

                _logger.LogInformation($"""
                                        HTTP response information:
                                        StatusCode: {httpContext.Response.StatusCode}
                                        ContentType: {httpContext.Response.ContentType}
                                        Headers: {FormatHeaders(httpContext.Response.Headers)}
                                        Body: {responseBodyText}
                                        """);

                newResponseBody.Seek(0, SeekOrigin.Begin);
                await newResponseBody.CopyToAsync(originalResponseBody);
            }
        }

        private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));

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
}
