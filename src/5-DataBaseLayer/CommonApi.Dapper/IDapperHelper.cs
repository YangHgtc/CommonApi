using System.Data;
using System.Data.Common;

namespace CommonApi.Dapper;


public interface IDapperHelper
{
    int Execute(string strSql, object? param = null);

    int Execute(string strSql, IDbTransaction trans, object? param = null);

    Task<int> ExecuteAsync(string strSql, object? param = null);

    Task<int> ExecuteAsync(string strSql, IDbTransaction trans, object? param = null);

    bool ExecuteFunc(Func<IDbTransaction, IDbConnection, int> func);

    T ExecuteFunc<T>(string funcName, object obj);

    Task<bool> ExecuteFuncAsync(Func<IDbTransaction, IDbConnection, Task<int>> func);

    Task<T> ExecuteFuncAsync<T>(string funcName, object obj);

    List<T> ExecuteFuncToList<T>(string funcName, object obj);

    Task<List<T>> ExecuteFuncToListAsync<T>(string funcName, object obj);

    int ExecuteStoredProcedure(string strProcedure, object? param = null);

    Task<int> ExecuteStoredProcedureAsync(string strProcedure, object? param = null);

    int ExecuteTransaction(string strSql);

    Task<int> ExecuteTransactionAsync(string strSql);

    DbConnection GetSqlConnection();

    List<T> QueryList<T>(string strSql, object? parm = null);

    Task<List<T>> QueryListAsync<T>(string strSql, object? param = null);

    T QueryFirstOrDefault<T>(string strSql, object? parm = null);

    Task<T> QueryFirstOrDefaultAsync<T>(string strSql, object? parm = null);

    (long, List<T>) QueryPagination<T>(string searchSql, string countSql);

    Task<(List<T> data, long total)> QueryPaginationAsync<T>(string searchSql, string countSql);

    T QueryScalar<T>(string sql, object? parms = null);

    Task<T> QueryScalarAsync<T>(string sql, object? parms = null);
}
