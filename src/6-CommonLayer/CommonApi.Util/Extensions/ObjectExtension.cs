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
    /// <param name="option">序列化选项</param>
    /// <typeparam name="T"><paramref name="obj"/></typeparam>
    /// <returns></returns>
    public static string Serialize<T>(this T? obj, JsonSerializerOptions? option = null)
    {
        return obj == null ? "null" : JsonSerializer.Serialize(obj, option ?? JsonHelper.JsonOptions);
    }

    /// <summary>
    /// 反序列化对象
    /// </summary>
    /// <param name="str"></param>
    /// <param name="option"></param>
    /// <typeparam name="T">序列化选项</typeparam>
    /// <returns></returns>
    public static T? DeSerialize<T>(this string str, JsonSerializerOptions? option = null)
    {
        return string.IsNullOrEmpty(str) ? default : JsonSerializer.Deserialize<T>(str, option ?? JsonHelper.JsonOptions);
    }
}
