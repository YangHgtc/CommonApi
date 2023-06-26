using System.Data;

namespace CommonApi.DataBase.Dapper;

public interface IDapperHelperAsync
{
    /// <summary>
    /// 获取数据库连接，注意使用using释放连接
    /// </summary>
    /// <returns></returns>
    Task<IDbConnection> GetSqlConnection();

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strSql"></param>
    /// <param name="parm"></param>
    /// <returns></returns>
    Task<List<T>> QueryAsync<T>(string strSql, object? parm = null);

    /// <summary>
    /// 执行SQL返回一个对象
    /// </summary>
    /// <param name="strSql">SQL语句</param>
    /// <param name="parm"></param>
    /// <returns></returns>
    Task<T> QueryFirstOrDefaultAsync<T>(string strSql, object? parm = null);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="parms"></param>
    /// <returns></returns>
    Task<T> QueryScalarAsync<T>(string sql, object? parms = null);

    /// <summary>
    /// 执行SQL
    /// </summary>
    /// <param name="strSql">SQL语句</param>
    /// <param name="param">参数</param>
    /// <returns>，0执行失败</returns>
    Task<int> ExecuteAsync(string strSql, object? param = null);

    /// <summary>
    /// 执行SQL
    /// </summary>
    /// <param name="strSql">SQL语句</param>
    /// <param name="trans"></param>
    /// <param name="param">参数</param>
    /// <returns>0执行失败</returns>
    Task<int> ExecuteAsync(string strSql, IDbTransaction trans, object? param = null);

    /// <summary>
    /// 执行存储过程
    /// </summary>
    /// <param name="strProcedure">过程名</param>
    /// <param name="param">参数</param>
    /// <returns></returns>
    Task<int> ExecuteStoredProcedureAsync(string strProcedure, object? param = null);

    /// <summary>
    /// 执行存储过程，返回集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="funcName"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    Task<List<T>> ExecuteFuncToListAsync<T>(string funcName, object obj);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="funcName"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    Task<T> ExecuteFuncAsync<T>(string funcName, object obj);

    /// <summary>
    ///
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    Task<bool> ExecuteFuncAsync(Func<IDbTransaction, IDbConnection, Task<int>> func);

    /// <summary>
    ///
    /// </summary>
    /// <param name="strSql"></param>
    /// <returns></returns>
    Task<int> ExecuteTransactionAsync(string strSql);
}
