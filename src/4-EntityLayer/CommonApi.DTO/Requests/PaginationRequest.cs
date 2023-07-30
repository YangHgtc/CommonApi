namespace CommonApi.DTO.Requests;

/// <summary>
/// 分页请求
/// </summary>
public class PaginationRequest
{
    /// <summary>
    /// 当前页
    /// </summary>
    public int Current { get; init; } = 1;

    /// <summary>
    /// 每页多少条数据
    /// </summary>
    public int PageSize { get; init; }
}
