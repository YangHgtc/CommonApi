using System.Data;
using MySql.Data.MySqlClient;

namespace CommonApi.DataBase;


public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync();
}

public sealed class MysqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public MysqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
