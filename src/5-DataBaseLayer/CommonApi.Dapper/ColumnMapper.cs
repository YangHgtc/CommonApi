using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Dapper;

namespace CommonApi.Dapper;

/// <summary>
/// 用于通过反射查找出所有的用到[colmun(Name="")]特性的model，自动增加映射
/// </summary>
public static class ColumnMapper
{
    /// <summary>
    /// 查找所有属性
    /// </summary>
    /// <param name="assembly"> </param>
    /// <param name="namespaceName"> 类型命名空间前缀 </param>
    /// <returns> </returns>
    /// <exception cref="ArgumentNullException"> </exception>
    public static void FindCustomAttributesPropertyInfo(Assembly assembly, string namespaceName)
    {
        assembly.GetTypes()
            .Where(type => !string.IsNullOrWhiteSpace(type.Namespace) &&
                           (type.Namespace.Equals(namespaceName, StringComparison.Ordinal) ||
                            type.Namespace.StartsWith(namespaceName + ".", StringComparison.Ordinal)))
            .AsParallel().ForAll(type =>
            {
                var propertyInfoList = type.GetProperties()
                    .Where(p => p.GetCustomAttribute(typeof(ColumnAttribute), false) != null).ToArray();
                if (propertyInfoList.Any())
                {
                    SqlMapper.SetTypeMap(type,
                        new ColumnAttributeTypeMapper(type, propertyInfoList)); //SetTypeMap方法内部已经加了锁
                }
            });
    }

    /// <summary>
    /// 查找所有属性
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public static void FindCustomAttributesPropertyInfo<T>() where T : class
    {
        //添加dapper实体类
        var assembly = typeof(T).Assembly;
        FindCustomAttributesPropertyInfo(assembly, assembly.GetName().Name!);
    }

    /// <summary>
    /// 查找所有属性
    /// </summary>
    /// <param name="assemblyName"> 类型的所在程序集 </param>
    /// <param name="namespaceName"> 类型命名空间前缀 </param>
    /// <returns> </returns>
    /// <exception cref="ArgumentNullException"> </exception>
    public static void FindCustomAttributesPropertyInfos(string assemblyName, string namespaceName)
    {
        //ConcurrentBag<(Type type, IEnumerable<PropertyInfo>)> properties = new();
        var assembly = Assembly.Load(assemblyName);
        if (assembly == null)
        {
            throw new ArgumentNullException("FindTypes assembly");
        }

        assembly.GetTypes()
            .Where(type => !string.IsNullOrWhiteSpace(type.Namespace) && (type.Namespace.Equals(namespaceName) ||
                                                                          type.Namespace.StartsWith(namespaceName +
                                                                              ".")))
            .AsParallel().ForAll(type =>
            {
                var propertyInfoList = type.GetProperties()
                    .Where(p => p.GetCustomAttribute(typeof(ColumnAttribute), false) != null).ToArray();
                if (propertyInfoList.Any())
                //properties.Add((type, propertyInfos));
                {
                    SqlMapper.SetTypeMap(type, new ColumnAttributeTypeMapper(type, propertyInfoList));
                }
            });
        //return properties;
    }

    /// <summary>
    /// 查找所有类型
    /// </summary>
    /// <param name="assemblyName"> 类型的所在程序集 </param>
    /// <param name="namespaceName"> 类型命名空间前缀 </param>
    /// <returns> </returns>
    public static IEnumerable<Type> FindCustomAttributesTypes(string assemblyName, string namespaceName)
    {
        var assembly = Assembly.Load(assemblyName) ?? throw new ArgumentNullException("FindTypes assembly");
        var types = assembly.GetTypes()
            .Where(type => !string.IsNullOrEmpty(type.Namespace) && (type.Namespace.Equals(namespaceName) ||
                                                                     type.Namespace.StartsWith(namespaceName + ".")))
            .Where(item =>
            {
                var propertyInfoList = item.GetProperties();
                return propertyInfoList.Any() &&
                       propertyInfoList.Any(p => p.GetCustomAttribute(typeof(ColumnAttribute)) != null);
            });
        var findCustomAttributesTypes = types.ToList();
        return !findCustomAttributesTypes.Any() ? throw new ArgumentNullException("FindTypes types") : (IEnumerable<Type>)findCustomAttributesTypes;
    }

    /// <summary>
    /// 用于通过反射查找出所有的用到[colmun(Name="")]特性的model，自动增加映射
    /// </summary>
    /// <param name="assemblyName"> 类型的所在程序集 </param>
    /// <param name="namespaceName"> 类型命名空间前缀 </param>
    public static void RegisterColumnAttributeTypeMapper(string assemblyName, string namespaceName)
    {
        if (!string.IsNullOrWhiteSpace(assemblyName) && !string.IsNullOrWhiteSpace(namespaceName))
        //二选其一
        //1
        //var typeList = FindCustomAttributesTypes(assemblyName, dataMapperNamespace);
        //typeList.AsParallel().ForAll(type => SqlMapper.SetTypeMap(type, new ColumnAttributeTypeMapper(type)));

        //2
        {
            FindCustomAttributesPropertyInfos(assemblyName, namespaceName);
        }
    }
}
