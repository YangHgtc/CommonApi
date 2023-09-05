using System.Data.Common;
using CommonApi.DataBase.Contracts;
using MySqlConnector;

namespace CommonApi.Mysql;

public sealed class MysqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public DataBaseType DataBaseName { get; set; } = DataBaseType.MySql;


    public DbConnection CreateConnection()
    {
        var connection = new MySqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}
