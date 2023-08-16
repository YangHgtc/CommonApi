using System.Data;
using Dapper;

namespace CommonApi.DataBase.Dapper;

public sealed class DapperHelperAsync(IDbConnectionFactory dbConnectionFactory) : IDapperHelperAsync
{
    /// <summary>
    /// 获取数据库连接，注意使用using释放连接
    /// </summary>
    /// <returns></returns>
    public async Task<IDbConnection> GetSqlConnection()
    {
        return await dbConnectionFactory.CreateConnectionAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strSql"></param>
    /// <param name="parm"></param>
    /// <returns></returns>
    public async Task<List<T>> QueryAsync<T>(string strSql, object? parm = null)
    {
        using var conn = await dbConnectionFactory.CreateConnectionAsync();
        var result = await conn.QueryAsync<T>(strSql, parm);
        return result.ToList();
    }

    /// <summary>
    /// 执行SQL返回一个对象
    /// </summary>
    /// <param name="strSql">SQL语句</param>
    /// <param name="parm"></param>
    /// <returns></returns>
    public async Task<T> QueryFirstOrDefaultAsync<T>(string strSql, object? parm = null)
    {
        using var conn = await dbConnectionFactory.CreateConnectionAsync();
        return await conn.QueryFirstOrDefaultAsync<T>(strSql, parm);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="parms"></param>
    /// <returns></returns>
    public async Task<T> QueryScalarAsync<T>(string sql, object? parms = null)
    {
        using var conn = await dbConnectionFactory.CreateConnectionAsync();
        return await conn.ExecuteScalarAsync<T>(sql, parms);
    }

    /// <summary>
    /// 执行SQL
    /// </summary>
    /// <param name="strSql">SQL语句</param>
    /// <param name="param">参数</param>
    /// <returns>，0执行失败</returns>
    public async Task<int> ExecuteAsync(string strSql, object? param = null)
    {
        using var conn = await dbConnectionFactory.CreateConnectionAsync();
        return await conn.ExecuteAsync(strSql, param);
    }

    /// <summary>
    /// 执行SQL
    /// </summary>
    /// <param name="strSql">SQL语句</param>
    /// <param name="trans"></param>
    /// <param name="param">参数</param>
    /// <returns>0执行失败</returns>
    public async Task<int> ExecuteAsync(string strSql, IDbTransaction trans, object? param = null)
    {
        using var conn = await dbConnectionFactory.CreateConnectionAsync();
        return await conn.ExecuteAsync(strSql, param, trans);
    }

    /// <summary>
    /// 执行存储过程
    /// </summary>
    /// <param name="strProcedure">过程名</param>
    /// <param name="param">参数</param>
    /// <returns></returns>
    public async Task<int> ExecuteStoredProcedureAsync(string strProcedure, object? param = null)
    {
        using var conn = await dbConnectionFactory.CreateConnectionAsync();
        return await conn.ExecuteAsync(strProcedure, param, null, null, CommandType.StoredProcedure);
    }

    /// <summary>
    /// 执行存储过程，返回集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="funcName"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public async Task<List<T>> ExecuteFuncToListAsync<T>(string funcName, object obj)
    {
        using var conn = await dbConnectionFactory.CreateConnectionAsync();

        var result = await conn.QueryAsync<T>(funcName, obj, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="funcName"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public async Task<T> ExecuteFuncAsync<T>(string funcName, object obj)
    {
        using var conn = await dbConnectionFactory.CreateConnectionAsync();

        return await conn.QueryFirstOrDefaultAsync<T>(funcName, obj, commandType: CommandType.StoredProcedure);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="strSql"></param>
    /// <returns></returns>
    public async Task<int> ExecuteTransactionAsync(string strSql)
    {
        using var conn = await dbConnectionFactory.CreateConnectionAsync();
        IDbTransaction trans = null;
        try
        {
            trans = conn.BeginTransaction();

            var iResult = await conn.ExecuteAsync(strSql, trans);
            if (iResult > 0)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }

            return iResult;
        }
        catch (Exception ex)
        {
            trans?.Rollback();
            return -1;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public async Task<bool> ExecuteFuncAsync(Func<IDbTransaction, IDbConnection, Task<int>> func)
    {
        using var conn = await dbConnectionFactory.CreateConnectionAsync();
        IDbTransaction trans = null;
        try
        {
            conn.Open();
            trans = conn.BeginTransaction();
            var iResult = await func.Invoke(trans, conn);

            if (iResult > 0)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }

            return iResult > 0;
        }
        catch (Exception ex)
        {
            //LogError(ex, "");
            trans?.Rollback();

            throw;
        }
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="searchSql"></param>
    /// <param name="countSql"></param>
    /// <returns></returns>
    public async Task<(long, List<T>)> QueryPaginationAsync<T>(string searchSql, string countSql)
    {
        using var conn = await dbConnectionFactory.CreateConnectionAsync();
        await using var res = await conn.QueryMultipleAsync(countSql + searchSql);
        var total = await res.ReadFirstAsync<long>();
        var data = (await res.ReadAsync<T>()).ToList();
        return (total, data);
    }
}
