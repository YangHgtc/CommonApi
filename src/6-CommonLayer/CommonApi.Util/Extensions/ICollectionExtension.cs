namespace CommonApi.Util.Extensions;

public static class ICollectionExtension
{
    /// <summary>
    /// 判断集合或数组是否不为空且数量大于0
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="collection"> </param>
    /// <returns> 如果集合或数组不为空且数量大于0则返回 <see langword="true" />,否则返回 <see langword="false" /> </returns>
    public static bool IsNotNullOrEmpty<T>(this ICollection<T> collection)
    {
        return collection is { Count: > 0 };
    }
}
