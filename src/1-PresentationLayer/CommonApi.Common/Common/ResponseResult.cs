namespace CommonApi.Common.Common;

/// <summary>
/// 统一返回结果
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed record ResponseResult<T>
{
    /// <summary>
    /// 自定义编码
    /// </summary>
    public required int Code { get; init; }
    /// <summary>
    /// 返回消息
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// 结果
    /// </summary>
    public T Data { get; set; } = default;
}
/// <summary>
/// 
/// </summary>
public enum ResponseStatusCode
{
    /// <summary>
    /// 
    /// </summary>
    Success = 0,
    /// <summary>
    /// 
    /// </summary>
    Fail = -1
}
