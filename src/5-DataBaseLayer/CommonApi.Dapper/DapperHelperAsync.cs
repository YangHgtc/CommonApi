using System.Data;
using CommonApi.DataBase.Contracts;
using Dapper;

namespace CommonApi.Dapper;

public partial class DapperHelper(IDbConnectionFactory dbConnectionFactory) : IDapperHelper
{
    protected IDbConnectionFactory DefaultDbConnectionFactory { get; set; } = dbConnectionFactory;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strSql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<List<T>> QueryListAsync<T>(string strSql, object? param = null)
    {
        await using var conn = DefaultDbConnectionFactory.CreateConnection();
        var result = await conn.QueryAsync<T>(strSql, param);
        return result.AsList();
    }

    /// <summary>
    /// 执行SQL返回一个对象
    /// </summary>
    /// <param name="strSql">SQL语句</param>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<T> QueryFirstOrDefaultAsync<T>(string strSql, object? param = null)
    {
        await using var conn = DefaultDbConnectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<T>(strSql, param);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<T> QueryScalarAsync<T>(string sql, object? param = null)
    {
        await using var conn = DefaultDbConnectionFactory.CreateConnection();
        return await conn.ExecuteScalarAsync<T>(sql, param);
    }

    /// <summary>
    /// 执行SQL
    /// </summary>
    /// <param name="strSql">SQL语句</param>
    /// <param name="param">参数</param>
    /// <returns>，0执行失败</returns>
    public async Task<int> ExecuteAsync(string strSql, object? param = null)
    {
        await using var conn = DefaultDbConnectionFactory.CreateConnection();
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
        await using var conn = DefaultDbConnectionFactory.CreateConnection();
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
        await using var conn = DefaultDbConnectionFactory.CreateConnection();
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
        await using var conn = DefaultDbConnectionFactory.CreateConnection();

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
        await using var conn = DefaultDbConnectionFactory.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<T>(funcName, obj, commandType: CommandType.StoredProcedure);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="strSql"></param>
    /// <returns></returns>
    public async Task<int> ExecuteTransactionAsync(string strSql)
    {
        await using var conn = DefaultDbConnectionFactory.CreateConnection();
        IDbTransaction trans = null;
        try
        {
            await conn.OpenAsync();
            trans = await conn.BeginTransactionAsync();

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
        finally
        {
            await conn.CloseAsync();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public async Task<bool> ExecuteFuncAsync(Func<IDbTransaction, IDbConnection, Task<int>> func)
    {
        await using var conn = DefaultDbConnectionFactory.CreateConnection();
        IDbTransaction trans = null;
        try
        {
            await conn.OpenAsync();
            trans = await conn.BeginTransactionAsync();
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
            trans?.Rollback();
            throw;
        }
        finally
        {
            await conn.CloseAsync();
        }
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="searchSql"></param>
    /// <param name="countSql"></param>
    /// <returns></returns>
    public async Task<(List<T> data, long total)> QueryPaginationAsync<T>(string searchSql, string countSql)
    {
        await using var conn = DefaultDbConnectionFactory.CreateConnection();
        await using var res = await conn.QueryMultipleAsync(searchSql + countSql);
        var data = (await res.ReadAsync<T>()).ToList();
        var total = await res.ReadFirstAsync<long>();
        return (data, total);
    }
}
