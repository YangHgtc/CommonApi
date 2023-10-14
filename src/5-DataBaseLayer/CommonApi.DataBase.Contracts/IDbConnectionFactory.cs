using System.Data.Common;
using System.Reflection;

namespace CommonApi.DataBase.Contracts;

public interface IDbConnectionFactory
{
    /// <summary>
    ///  创建数据库连接
    /// </summary>
    /// <remarks>
    ///  除存储过程需要手动打开数据库连接外，其他操作不需要手动打开数据库连接，dapper会自动打开
    /// </remarks>
    /// <returns></returns>
    public DbConnection CreateConnection();
}

/// <summary>
/// 抽象数据库连接工厂接口
/// </summary>
public interface IAbstractConnectionFactory
{
    /// <summary>
    /// 创建数据库连接工厂
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    IDbConnectionFactory CreateConnectionFactory(DataBaseType name = DataBaseType.Default);
}

/// <summary>
/// 抽象数据库连接工厂
/// </summary>
public sealed class AbstractConnectionFactory(Func<IEnumerable<IDbConnectionFactory>> factory) : IAbstractConnectionFactory
{

    /// <inheritdoc/>
    public IDbConnectionFactory CreateConnectionFactory(DataBaseType name = DataBaseType.Default)
    {
        var set = factory();
        var output = name == DataBaseType.Default
            ? set.First()
            : set.First(x => x.GetType().GetCustomAttribute<DataBaseNameAttribute>()!.Name == name);
        return output;
    }
}
