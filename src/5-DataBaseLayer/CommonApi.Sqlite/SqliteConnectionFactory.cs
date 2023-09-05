using System.Data.Common;
using CommonApi.DataBase.Contracts;
using Microsoft.Data.Sqlite;

namespace CommonApi.Sqlite;

public sealed class SqliteConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public DataBaseType DataBaseName { get; set; } = DataBaseType.Sqlite;

    public DbConnection CreateConnection()
    {
        var connection = new SqliteConnection(connectionString);
        return connection;
    }
}
