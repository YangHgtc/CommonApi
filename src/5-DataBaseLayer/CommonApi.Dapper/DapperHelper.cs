using System.Data;
using System.Data.Common;
using CommonApi.DataBase.Contracts;
using Dapper;

namespace CommonApi.Dapper;

public partial class DapperHelper : IDapperHelper
{
    /// <summary>
    ///     获取数据库连接，注意使用using释放连接
    /// </summary>
    /// <returns></returns>
    public virtual DbConnection GetDbConnection(DataBaseType name = DataBaseType.Default)
    {
        var factory = dbConnectionConnectionFactory.CreateConnectionFactory(name);
        return factory.CreateConnection();
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public List<T> QueryList<T>(string sql, object? param = null)
    {
        using var conn = _connectionFactory.CreateConnection();
        var result = conn.Query<T>(sql, param);
        return result.AsList();
    }

    /// <summary>
    ///     执行SQL返回一个对象
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="param"></param>
    /// <returns></returns>
    public T QueryFirstOrDefault<T>(string sql, object? param = null)
    {
        using var conn = _connectionFactory.CreateConnection();
        return conn.QueryFirstOrDefault<T>(sql, param);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public T QueryScalar<T>(string sql, object? param = null)
    {
        using var conn = _connectionFactory.CreateConnection();
        return conn.ExecuteScalar<T>(sql, param);
    }

    /// <summary>
    ///     执行SQL
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="param">参数</param>
    /// <returns>，0执行失败</returns>
    public int Execute(string sql, object? param = null)
    {
        using var conn = _connectionFactory.CreateConnection();
        return conn.Execute(sql, param);
    }

    /// <summary>
    ///     执行SQL
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="trans"></param>
    /// <param name="param">参数</param>
    /// <returns>0执行失败</returns>
    public int Execute(string sql, IDbTransaction trans, object? param = null)
    {
        using var conn = _connectionFactory.CreateConnection();
        return conn.Execute(sql, param, trans);
    }

    /// <summary>
    ///     执行存储过程
    /// </summary>
    /// <param name="strProcedure">过程名</param>
    /// <param name="param">参数</param>
    /// <returns></returns>
    public int ExecuteStoredProcedure(string strProcedure, object? param = null)
    {
        using var conn = _connectionFactory.CreateConnection();
        return conn.Execute(strProcedure, param, null, null, CommandType.StoredProcedure);
    }

    /// <summary>
    ///     执行存储过程，返回集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="funcName"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public List<T> ExecuteFuncToList<T>(string funcName, object obj)
    {
        using var conn = _connectionFactory.CreateConnection();

        var result = conn.Query<T>(funcName, obj, commandType: CommandType.StoredProcedure);
        return result.AsList();
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="funcName"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public T ExecuteFunc<T>(string funcName, object obj)
    {
        using var conn = _connectionFactory.CreateConnection();

        return conn.QueryFirstOrDefault<T>(funcName, obj, commandType: CommandType.StoredProcedure);
    }

    /// <summary>
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public int ExecuteTransaction(string sql)
    {
        using var conn = _connectionFactory.CreateConnection();
        IDbTransaction trans = null;
        try
        {
            conn.Open();
            trans = conn.BeginTransaction();

            var iResult = conn.Execute(sql, trans);
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
            conn.Close();
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public bool ExecuteFunc(Func<IDbTransaction, IDbConnection, int> func)
    {
        using var conn = _connectionFactory.CreateConnection();
        IDbTransaction trans = null;
        try
        {
            conn.Open();
            trans = conn.BeginTransaction();
            var iResult = func.Invoke(trans, conn);

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
        finally
        {
            conn.Close();
        }
    }
}
