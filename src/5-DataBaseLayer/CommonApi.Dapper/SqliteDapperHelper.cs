using CommonApi.DataBase.Contracts;

namespace CommonApi.Dapper;

public sealed class SqliteDapperHelper(IEnumerable<IDbConnectionFactory> dbConnectionFactory) : DapperHelper(dbConnectionFactory.First(x => x.DataBaseName == DataBaseType.Sqlite))
{
}
