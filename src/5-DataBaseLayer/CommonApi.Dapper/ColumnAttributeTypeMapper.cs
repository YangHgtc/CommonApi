using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Dapper;

namespace CommonApi.Dapper;

public sealed class ColumnAttributeTypeMapper<T>() : FallbackTypeMapper(new SqlMapper.ITypeMap[]
{
    new CustomPropertyTypeMap(
        typeof(T),
        (type, columnName) =>
            type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false)
                    .OfType<ColumnAttribute>()
                    .Any(attr => attr.Name == columnName)
            )
    ),
    new DefaultTypeMap(typeof(T))
});

/// <summary>
/// 我自定义的映射
/// </summary>
public sealed class ColumnAttributeTypeMapper : FallbackTypeMapper
{
    public ColumnAttributeTypeMapper(Type type)
        : base(new SqlMapper.ITypeMap[]
        {
            new CustomPropertyTypeMap(
                type,
                (type1, columnName) =>
                    type1.GetProperties().FirstOrDefault(prop =>
                        prop.GetCustomAttributes(false)
                            .OfType<ColumnAttribute>()
                            .Any(attr => attr.Name == columnName)
                    )
            ),
            new DefaultTypeMap(type)
        })
    {
    }

    public ColumnAttributeTypeMapper(Type type, IEnumerable<PropertyInfo> propertyInfos)
        : base(new SqlMapper.ITypeMap[]
        {
            new CustomPropertyTypeMap(type, (_, columnName) =>
                propertyInfos.FirstOrDefault(prop =>
                    (prop.GetCustomAttribute(typeof(ColumnAttribute), false) as ColumnAttribute)?.Name == columnName
                )
            ),
            new DefaultTypeMap(type)
        })
    {
    }
}

public class FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers) : SqlMapper.ITypeMap
{
    public ConstructorInfo FindConstructor(string[] names, Type[] types)
    {
        foreach (var mapper in mappers)
        {
            try
            {
                var result = mapper.FindConstructor(names, types);
                if (result != null)
                {
                    return result;
                }
            }
            catch (NotImplementedException)
            {
            }
        }

        return null;
    }

    public ConstructorInfo FindExplicitConstructor()
    {
        return mappers
            .Select(mapper => mapper.FindExplicitConstructor())
            .FirstOrDefault(result => result != null);
    }

    public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
    {
        foreach (var mapper in mappers)
        {
            try
            {
                var result = mapper.GetConstructorParameter(constructor, columnName);
                if (result != null)
                {
                    return result;
                }
            }
            catch (NotImplementedException)
            {
            }
        }

        return null;
    }

    public SqlMapper.IMemberMap GetMember(string columnName)
    {
        foreach (var mapper in mappers)
        {
            try
            {
                var result = mapper.GetMember(columnName);
                if (result != null)
                {
                    return result;
                }
            }
            catch (NotImplementedException)
            {
            }
        }

        return null;
    }
}
