using System.Data;
using System.Data.Common;
using CommonApi.DataBase.Contracts;
using Dapper;

namespace CommonApi.Dapper;

public interface IDapperHelper
{
    int Execute(string sql, object? param = null);

    int Execute(string sql, IDbTransaction trans, object? param = null);

    Task<int> ExecuteAsync(string sql, object? param = null);

    Task<int> ExecuteAsync(string sql, IDbTransaction trans, object? param = null);

    bool ExecuteFunc(Func<IDbTransaction, IDbConnection, int> func);

    T ExecuteFunc<T>(string funcName, object obj);

    Task<bool> ExecuteFuncAsync(Func<IDbTransaction?, IDbConnection, Task<int>> func);

    Task<T?> ExecuteFuncAsync<T>(string funcName, object obj);

    List<T> ExecuteFuncToList<T>(string funcName, object obj);

    Task<List<T>> ExecuteFuncToListAsync<T>(string funcName, object obj);

    int ExecuteStoredProcedure(string strProcedure, object? param = null);

    Task<int> ExecuteStoredProcedureAsync(string strProcedure, object? param = null);

    int ExecuteTransaction(string sql);

    Task<int> ExecuteTransactionAsync(string sql);

    DbConnection GetDbConnection(DataBaseType name = DataBaseType.Default);

    List<T> QueryList<T>(string sql, object? parm = null);

    Task<List<T>> QueryListAsync<T>(string sql, object? param = null);

    T QueryFirstOrDefault<T>(string sql, object? parm = null);

    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parm = null);

    Task<(List<T> data, int total)> QueryPaginationAsync<T>(string sql, int currentPage, int pageSize);

    T QueryScalar<T>(string sql, object? parms = null);

    Task<T?> QueryScalarAsync<T>(string sql, object? parms = null);

    Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null);
}
