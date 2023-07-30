using System.Text.Json;
using CommonApi.Util.Helpers;

namespace CommonApi.Util.Extensions;

/// <summary>
///
/// </summary>
public static class ObjectExtension
{
    /// <summary>
    ///  序列化对象
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string Serialize<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, JsonHelper.JsonOptions);
    }

    /// <summary>
    /// 反序列化对象
    /// </summary>
    /// <param name="str"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? DeSerialize<T>(this string str)
    {
        return JsonSerializer.Deserialize<T>(str, JsonHelper.JsonOptions);
    }
}
