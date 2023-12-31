using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CommonApi.Common.Extensions;

/// <summary>
/// 日志源生成器
/// </summary>
public static partial class LogExtension
{
    /// <summary>
    /// 记录http请求
    /// </summary>
    /// <param name="logger">日志</param>
    /// <param name="method">http方法</param>
    /// <param name="path">http路径</param>
    /// <param name="queryString">请求字符串</param>
    /// <param name="headers">请求头</param>
    /// <param name="scheme"></param>
    /// <param name="host">请求地址</param>
    /// <param name="body">请求体</param>
    [LoggerMessage(
        EventId = 0,
        SkipEnabledCheck = true,
        Level = LogLevel.Information,
        Message = """
                    HTTP request information:
                    Method: {Method}
                    Path: {Path}
                    QueryString: {QueryString}
                    Headers: {Headers}
                    Schema: {Scheme}
                    Host: {Host}
                    Body: {Body}
                  """)]
    public static partial void LogRequest(
        this ILogger logger,
        string method,
        PathString path,
        QueryString queryString,
        IHeaderDictionary headers,
        string scheme,
        HostString host,
        string body);

    /// <summary>
    /// 记录响应
    /// </summary>
    /// <param name="logger">日志</param>
    /// <param name="path">路径</param>
    /// <param name="statusCode">状态码</param>
    /// <param name="contentType">内容类型</param>
    /// <param name="headers">头</param>
    /// <param name="body">响应体</param>
    [LoggerMessage(
        EventId = 1,
        SkipEnabledCheck = true,
        Level = LogLevel.Information,
        Message = """
                     HTTP response information:
                     Path: {Path}
                     StatusCode: {StatusCode}
                     ContentType: {ContentType}
                     Headers: {Headers}
                     Body: {Body}
                  """)]
    public static partial void LogResponse(
        this ILogger logger,
        string path,
        int statusCode,
        string? contentType,
        IHeaderDictionary headers,
        string body);
}
