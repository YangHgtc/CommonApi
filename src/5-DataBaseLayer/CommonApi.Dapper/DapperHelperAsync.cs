using System.Data;
using CommonApi.DataBase.Contracts;
using Dapper;

namespace CommonApi.Dapper;

public partial class DapperHelper(IAbstractFactory dbConnectionFactory) : IDapperHelper
{
    private readonly IDbConnectionFactory _connectionFactory = dbConnectionFactory.Create(DataBaseType.MySql);

    /// <summary>
    /// 执行SQL返回一个List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<List<T>> QueryListAsync<T>(string sql, object? param = null)
    {
        await using var conn = _connectionFactory.CreateConnection();
        var result = await conn.QueryAsync<T>(sql, param);
        return result.AsList();
    }

    /// <summary>
    /// 执行SQL返回一个对象
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
    {
        await using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<T>(sql, param);
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
        await using var conn = _connectionFactory.CreateConnection();
        return await conn.ExecuteScalarAsync<T>(sql, param);
    }

    /// <summary>
    /// 执行SQL
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="param">参数</param>
    /// <returns>，0执行失败</returns>
    public async Task<int> ExecuteAsync(string sql, object? param = null)
    {
        await using var conn = _connectionFactory.CreateConnection();
        return await conn.ExecuteAsync(sql, param);
    }

    /// <summary>
    /// 执行SQL
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="trans"></param>
    /// <param name="param">参数</param>
    /// <returns>0执行失败</returns>
    public async Task<int> ExecuteAsync(string sql, IDbTransaction trans, object? param = null)
    {
        await using var conn = _connectionFactory.CreateConnection();
        return await conn.ExecuteAsync(sql, param, trans);
    }

    /// <summary>
    /// 执行存储过程
    /// </summary>
    /// <param name="strProcedure">过程名</param>
    /// <param name="param">参数</param>
    /// <returns></returns>
    public async Task<int> ExecuteStoredProcedureAsync(string strProcedure, object? param = null)
    {
        await using var conn = _connectionFactory.CreateConnection();
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
        await using var conn = _connectionFactory.CreateConnection();

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
        await using var conn = _connectionFactory.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<T>(funcName, obj, commandType: CommandType.StoredProcedure);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public async Task<int> ExecuteTransactionAsync(string sql)
    {
        await using var conn = _connectionFactory.CreateConnection();
        IDbTransaction trans = null;
        try
        {
            await conn.OpenAsync();
            trans = await conn.BeginTransactionAsync();

            var iResult = await conn.ExecuteAsync(sql, trans);
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
    /// 执行函数
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public async Task<bool> ExecuteFuncAsync(Func<IDbTransaction, IDbConnection, Task<int>> func)
    {
        await using var conn = _connectionFactory.CreateConnection();
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
    /// <param name="sql">sql</param>
    /// <param name="currentPage">当前页</param>
    /// <param name="pageSize">每页多少条</param>
    /// <returns></returns>
    public async Task<(List<T> data, int total)> QueryPaginationAsync<T>(string sql, int currentPage, int pageSize)
    {
        await using var conn = _connectionFactory.CreateConnection();
        var res = await conn.QueryAsync<T>(sql);
        var data = res.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        var total = res.TryGetNonEnumeratedCount(out var count) ? count : 0;
        return (data, total);
    }

    /// <summary>
    /// 一次查询多条sql
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// 记得使用using释放资源,使用方式如下
    /// <code>
    /// using var res = await QueryMultipleAsync(sql,param);
    /// var first = await res.ReadFirstAsync{T}();
    /// </code>
    ///
    /// </remarks>
    public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object? param = null)
    {
        await using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryMultipleAsync(sql, param);
    }
}
