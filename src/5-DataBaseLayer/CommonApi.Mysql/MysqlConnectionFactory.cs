using System.Data.Common;
using CommonApi.DataBase.Contracts;
using MySqlConnector;

namespace CommonApi.Mysql;

[DataBaseName(Name = DataBaseType.MySql)]
public sealed class MysqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public DbConnection CreateConnection()
    {
        var connection = new MySqlConnection(connectionString);
        return connection;
    }
}
