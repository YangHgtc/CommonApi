using System.Globalization;
using CommonApi.Util.Helpers;

namespace CommonApi.Util.Extensions;

public static class DatetimeExtension
{
    /// <summary>
    /// 返回 yyyy-MM-dd HH:mm:ss 格式的时间
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToFormattedString(this DateTime dt)
    {
        return dt.ToString(TimeHelper.DateTimeFormatter);
    }

    /// <summary>
    /// 本周第一天
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime FirstDayOfWeek(this DateTime dt)
    {
        var currentCulture = CultureInfo.CurrentCulture;
        var firstDayOfWeek = currentCulture.DateTimeFormat.FirstDayOfWeek;
        var offset = dt.DayOfWeek - firstDayOfWeek < 0 ? 7 : 0;
        var numberOfDaysSinceBeginningOfTheWeek = dt.DayOfWeek + offset - firstDayOfWeek;
        return dt.AddDays(-numberOfDaysSinceBeginningOfTheWeek);
    }

    /// <summary>
    /// 本月第一天凌晨
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime FirstDayOfMonth(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, 0, 0);
    }

    /// <summary>
    /// 判断当前时间是否在比较时间之前
    /// </summary>
    /// <param name="current">The current value.</param>
    /// <param name="toCompareWith">Value to compare with.</param>
    /// <returns>
    ///
    /// </returns>
    public static bool IsBefore(this DateTime current, DateTime toCompareWith) =>
        current < toCompareWith;

    /// <summary>
    /// 判断当前时间是否在比较时间之后
    /// </summary>
    /// <param name="current">The current value.</param>
    /// <param name="toCompareWith">Value to compare with.</param>
    /// <returns>
    /// 	
    /// </returns>
    public static bool IsAfter(this DateTime current, DateTime toCompareWith) =>
        current > toCompareWith;
}
