using Dapper;

namespace CommonApi.DataBase.Dapper;

public static class DapperExtension
{
    /// <summary>
    /// 分页查询;sql顺序为data;total;
    /// </summary>
    /// <param name="dapperHelper"></param>
    /// <param name="strSql">sql语句</param>
    /// <returns></returns>
    public static async Task<(List<T> data, int total)> PaginationQueryAsync<T>(this DapperHelperAsync dapperHelper, string strSql)
    {
        using var conn = await dapperHelper.GetSqlConnection();
        conn.Open();
        await using var result = await conn.QueryMultipleAsync(strSql);
        var data = (await result.ReadAsync<T>()).ToList();
        var total = await result.ReadFirstAsync<long>();
        return (data, (int)total);
    }
}
