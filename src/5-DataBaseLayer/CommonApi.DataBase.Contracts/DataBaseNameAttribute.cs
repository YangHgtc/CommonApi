namespace CommonApi.DataBase.Contracts;

/// <summary>
/// 用来标记数据库工厂类型
/// </summary>
[AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
public sealed class DataBaseNameAttribute : Attribute
{
    /// <summary>
    /// 数据库类型名
    /// </summary>
    public DataBaseType Name { get; set; }
}
