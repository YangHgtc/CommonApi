using System.ComponentModel;
using EnumsNET;

namespace CommonApi.Util.Extensions;

/// <summary>
///     枚举扩展
/// </summary>
public static class EnumExtension
{
    /// <summary>
    ///     快速获取枚举字符串
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static string ToStringFast<TEnum>(this TEnum value) where TEnum : struct, Enum
    {
        return value.AsString();
    }

    /// <summary>
    ///     快速把枚举转为int
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static int ToIntFast<TEnum>(this TEnum value) where TEnum : struct, Enum
    {
        return Convert.ToInt32(value);
    }

    /// <summary>
    ///     快速获取枚举的description特性
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static string GetDescriptionFast<TEnum>(this TEnum value) where TEnum : struct, Enum
    {
        return value.GetMember()!.Attributes.Get<DescriptionAttribute>()!.Description;
    }
}
