using CommonApi.Util.Extensions;

namespace CommonApi.Util.Helpers;

public static class TimeHelper
{
    public const string Today = nameof(Today);
    public const string Week = nameof(Week);
    public const string Month = nameof(Month);

    /// <summary>
    ///     yyyy-MM-dd HH:mm:ss
    /// </summary>
    public const string DateTimeFormatter = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    ///     yyyy-MM-dd HH:mm
    /// </summary>
    public const string DateTimeFormatterOfMin = "yyyy-MM-dd HH:mm";

    /// <summary>
    ///     yyyy-MM-dd HH
    /// </summary>
    public const string DateTimeFormatterOfHour = "yyyy-MM-dd HH";

    /// <summary>
    ///     获取格式化后的当前时间
    /// </summary>
    /// <returns></returns>
    public static string GetNow()
    {
        return DateTime.Now.ToFormattedString();
    }
}
