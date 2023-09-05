using System.Data.Common;

namespace CommonApi.DataBase.Contracts;

public interface IDbConnectionFactory
{
    /// <summary>
    /// 数据库名
    /// </summary>
    public DataBaseType DataBaseName { get; set; }

    /// <summary>
    /// 创建数据库连接
    /// </summary>
    /// <remarks>
    /// 除存储过程需要手动打开数据库连接外，其他操作不需要手动打开数据库连接，dapper会自动打开
    /// </remarks>
    /// <returns></returns>
    public DbConnection CreateConnection();
}
