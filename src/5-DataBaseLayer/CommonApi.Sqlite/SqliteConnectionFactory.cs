using System.Data.Common;
using CommonApi.DataBase.Contracts;
using Microsoft.Data.Sqlite;

namespace CommonApi.Sqlite;

[DataBaseName(Name = DataBaseType.Sqlite)]
public sealed class SqliteConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public DbConnection CreateConnection()
    {
        var connection = new SqliteConnection(connectionString);
        return connection;
    }
}
