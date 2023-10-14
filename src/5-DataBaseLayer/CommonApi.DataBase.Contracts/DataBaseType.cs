namespace CommonApi.DataBase.Contracts;

public enum DataBaseType
{
    /// <summary>
    ///     第一个注册的数据库
    /// </summary>
    Default,

    /// <summary>
    ///     MySql
    /// </summary>
    MySql,

    /// <summary>
    ///     Sqlite
    /// </summary>
    Sqlite,

    /// <summary>
    ///     PostgreSQL
    /// </summary>
    PostgreSQL,

    /// <summary>
    ///     MonogoDb
    /// </summary>
    MonogoDb,

    /// <summary>
    ///     LiteDb
    /// </summary>
    LiteDb
}
