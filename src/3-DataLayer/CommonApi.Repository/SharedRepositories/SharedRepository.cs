using CommonApi.Dapper;
using Microsoft.Extensions.Logging;

namespace CommonApi.Repository.SharedRepositories;

/// <summary>
/// 共享仓储
/// </summary>
/// <param name="dapper"></param>
/// <param name="logger"></param>
public sealed class SharedRepository(IDapperHelper dapper, ILogger<SharedRepository> logger) : ISharedRepository
{
    public async Task<List<string>> GetSomeDataAsync()
    {
        //使用dapper参数化方式
        var sql = "SELECT * FROM TABLE WHERE ID = @ID";
        logger.LogInformation("sql is:{sql}", sql);
        var res = await dapper.QueryListAsync<string>(sql, new { ID = 1 });
        return res;
    }
}

public interface ISharedRepository
{
    Task<List<string>> GetSomeDataAsync();
}
