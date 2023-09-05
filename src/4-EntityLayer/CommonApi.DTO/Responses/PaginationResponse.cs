namespace CommonApi.DTO.Responses;

/// <summary>
/// 分页响应
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginationResponse<T>
{
    /// <summary>
    /// 总数
    /// </summary>
    public int Total { get; init; } = 0;

    /// <summary>
    /// 数据
    /// </summary>
    public required IReadOnlyCollection<T> Records { get; init; } = Array.Empty<T>();
}
