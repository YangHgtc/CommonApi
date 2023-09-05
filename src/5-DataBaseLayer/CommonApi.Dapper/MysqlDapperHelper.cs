using CommonApi.DataBase.Contracts;

namespace CommonApi.Dapper;

public sealed class MysqlDapperHelper(IEnumerable<IDbConnectionFactory> dbConnectionFactory) : DapperHelper(dbConnectionFactory.First(x => x.DataBaseName == DataBaseType.MySql))
{
}
